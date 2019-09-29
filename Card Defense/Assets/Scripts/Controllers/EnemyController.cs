using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
[RequireComponent(typeof(AILerp))]
[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(BoxCollider))]
public class EnemyController : MonoBehaviour
{
	public float health, speed;
	private Animator animator;
	private AILerp AILerp;
	private Enemy enemy;
	public EnemyType enemyType;
	public int livesCost;

	private void Awake()
	{
		AILerp = GetComponent<AILerp>();
		animator = GetComponent<Animator>();
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
		AILerp.speed = speed * GameManager.gameSpeedMultiplier;
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
		CodeControl.Message.RemoveListener<GameSpeedChangedEvent>(OnGameSpeedChanged);
		Destroy(gameObject);
	}

	internal void Initialize(Enemy enemy, Vector3 startingPoint, Vector3 destination)
	{
		if (AILerp == null) AILerp = GetComponent<AILerp>();
		this.enemy = enemy;
		destination *= 2;
		transform.position = startingPoint;
		AILerp.destination = destination;
		AILerp.SearchPath();
		animator.SetBool("Moving", true);
	}

	private void DispatchEnemyReachedDestinationEvent()
	{
		CodeControl.Message.Send(new EnemyReachedDestinationEvent(this));
	}
}
