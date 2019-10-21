using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.EventSystems;
[RequireComponent(typeof(AILerp))]
[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(BoxCollider))]
public class EnemyController : MonoBehaviour, IPointerDownHandler
{
	public float health, speed;
	private Animator animator;
	private AILerp AILerp;
	private Enemy enemy;
	public EnemyType enemyType;
	public int livesCost;
	private float slowMultiplier, slowDuration;
	private int randomStart, randomEnd;
	private Tile[] startingPoints;
	private Tile[] endingPoints;

	private void Awake()
	{
		AILerp = GetComponent<AILerp>();
		animator = GetComponent<Animator>();
		slowMultiplier = 1;
		CodeControl.Message.AddListener<GameSpeedChangedEvent>(OnGameSpeedChanged);
		AILerp.onTargetReached += OnPathEndReached;
		SetSpeedValues();
		this.enemy = new Enemy(enemyType, EnemySpecialAbility.None);
	}

	private void OnGameSpeedChanged(GameSpeedChangedEvent obj)
	{
		SetSpeedValues();
	}

	public void Initialize(Enemy enemy, Tile[] startingPositions, Tile[] endingPositions)
	{
		if (AILerp == null) AILerp = GetComponent<AILerp>();
		this.enemy = enemy;
		this.enemy.transform = transform;
		this.enemy.maxHealth = health;
		this.enemy.currentHealth = health;
		this.enemy.speed = speed;
		RandomlyChoosePath(startingPositions, endingPositions);
		animator.SetBool("Moving", true);
	}

	private void SetSpeedValues()
	{
		animator.SetFloat("SpeedMultiplier", speed * GameManager.gameSpeedMultiplier);
		AILerp.speed = speed * slowMultiplier * GameManager.gameSpeedMultiplier;
	}

	public void SlowEnemy(float multiplier, float duration)
	{
		AILerp.speed *= multiplier;
		slowMultiplier = multiplier;
		slowDuration = duration;
		StartCoroutine(Slow());
	}

	private IEnumerator Slow()
	{
		while(slowDuration > 0)
		{
			yield return new WaitForSeconds(0.5f);
			CodeControl.Message.Send(new ElementalContactParticleRequestEvent(Element.Ice, transform.position));
		}
		yield return null;
	}

	private void Update()
	{
		if (slowDuration > 0)
		{
			slowDuration -= Time.deltaTime * GameManager.gameSpeedMultiplier;
		}
		else
		{
			if (slowMultiplier != 1)
			{
				slowMultiplier = 1;
				SetSpeedValues();
			}
		}
	}

	private void OnPathEndReached()
	{
		DispatchEnemyReachedDestinationEvent();
		Die();
	}

	public void Damage(float damage)
	{
		health -= damage;
		enemy.currentHealth = health;
		enemy.OnHealthChanged?.Invoke();
		if (health <= 0)
		{
			Die();
		}
		else
		{
			animator.SetTrigger("Damage");
		}
	}

	private void Die()
	{
		StartCoroutine(DeathWait());
	}

	private IEnumerator DeathWait()
	{
		animator.SetTrigger("Die");
		yield return !animator.GetCurrentAnimatorStateInfo(0).IsName("Die");
		CodeControl.Message.Send(new EnemyDeathEvent(this));
		CodeControl.Message.RemoveListener<GameSpeedChangedEvent>(OnGameSpeedChanged);
		Destroy(gameObject);
	}

	private void RandomlyChoosePath(Tile[] startingPoints, Tile[] endingPoints)
	{
		randomStart = UnityEngine.Random.Range(0, startingPoints.Length);
		randomEnd = UnityEngine.Random.Range(0, endingPoints.Length);
		this.startingPoints = startingPoints;
		this.endingPoints = endingPoints;
		Path path = ABPath.Construct(startingPoints[randomStart].Vector3FromCoordinates, endingPoints[randomEnd].Vector3FromCoordinates, OnPathCalculationDone);
		AILerp.SetPath(path);
	}

	private void OnPathCalculationDone(Path path)
	{
		if (path.PipelineState == PathState.Returned)
		{
			AILerp.SetPath(path);
		}
		else if (path.CompleteState == PathCompleteState.Error)
		{
			if (randomStart < startingPoints.Length)
			{
				randomStart++;
			}
			else if (randomStart > 0)
			{
				randomStart--;
			}
			if (randomEnd < endingPoints.Length)
			{
				randomEnd++;
			}
			else if (randomStart > 0)
			{
				randomEnd--;
			}
			Path nextPath = ABPath.Construct(startingPoints[randomStart].Vector3FromCoordinates, endingPoints[randomEnd].Vector3FromCoordinates, OnPathCalculationDone);
			AILerp.SetPath(path);
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if(eventData.button == PointerEventData.InputButton.Left)
		{
			DispatchEnemyInfoUIRequestEvent(enemy);
		}
	}
	//maybe I need to update the speed if the enemy is slowed?
	private void DispatchEnemyInfoUIRequestEvent(Enemy enemy)
	{		
		CodeControl.Message.Send(new EnemyInfoUIDisplayRequestEvent(enemy));
	}

	private void DispatchEnemyReachedDestinationEvent()
	{
		CodeControl.Message.Send(new EnemyReachedDestinationEvent(this));
	}
}
