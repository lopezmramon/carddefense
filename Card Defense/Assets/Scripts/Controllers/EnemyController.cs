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

	private void Awake()
	{
		AILerp = GetComponent<AILerp>();
		animator = GetComponent<Animator>();
		slowMultiplier = 1;
		CodeControl.Message.AddListener<GameSpeedChangedEvent>(OnGameSpeedChanged);
		AILerp.onTargetReached += OnPathEndReached;
		SetSpeedValues();
		this.enemy = new Enemy(enemyType, EnemySpecialAbility.None);
		enemy.livesCost = livesCost;
	}

	private void OnGameSpeedChanged(GameSpeedChangedEvent obj)
	{
		SetSpeedValues();
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

	public void Initialize(Enemy enemy, Path path)
	{
		if (AILerp == null) AILerp = GetComponent<AILerp>();
		this.enemy = enemy;
		AILerp.SetPath(path);
		if (path.path == null) AILerp.seeker.StartPath(path);
		animator.SetBool("Moving", true);
	}

	private void DispatchEnemyReachedDestinationEvent()
	{
		CodeControl.Message.Send(new EnemyReachedDestinationEvent(this));
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if(eventData.button == PointerEventData.InputButton.Left)
		{

		}
	}
}
