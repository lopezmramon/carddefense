using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	private List<EnemyController> enemies = new List<EnemyController>();
	private bool lastEnemySpawned = false;
	private void Awake()
	{
		CodeControl.Message.AddListener<EnemyDeathEvent>(OnEnemyDeath);
		CodeControl.Message.AddListener<EnemyReachedDestinationEvent>(OnEnemyReachedDestination);
		CodeControl.Message.AddListener<EnemySpawnedEvent>(OnEnemySpawned);
		CodeControl.Message.AddListener<EnemyDamageRequestEvent>(OnEnemyDamageRequested);
		CodeControl.Message.AddListener<DamageEnemiesRequestEvent>(OnDamageEnemiesRequested);
		CodeControl.Message.AddListener<SlowEnemiesRequestEvent>(OnSlowEnemiesRequested);
		CodeControl.Message.AddListener<LastEnemyInWaveSpawnedEvent>(OnLastEnemySpawned);
		CodeControl.Message.AddListener<WaveStartedEvent>(OnWaveStart);
	}

	private void OnSlowEnemiesRequested(SlowEnemiesRequestEvent obj)
	{
		SlowEnemies(obj.all, obj.amount, obj.duration, obj.slowAmount);
	}

	private void SlowEnemies(bool all, int amount, float duration, float slowAmount)
	{

		if (all)
		{
			foreach (EnemyController enemy in enemies)
			{
				if (enemy == null) continue;
				enemy.SlowEnemy(1 - slowAmount / 100, duration);
			}
		}
		else
		{
			int enemiesDamaged = 0;
			if (enemies.Count < amount) amount = enemies.Count;
			while (enemiesDamaged < amount)
			{
				enemiesDamaged++;
				int randomEnemyIndex = UnityEngine.Random.Range(0, enemies.Count);
				while (enemies[randomEnemyIndex] == null) randomEnemyIndex = UnityEngine.Random.Range(0, enemies.Count);
				enemies[randomEnemyIndex].SlowEnemy(1 - slowAmount / 100, duration);
			}
		}
	}

	private void OnDamageEnemiesRequested(DamageEnemiesRequestEvent obj)
	{
		DamageEnemies(obj.all, obj.amount, obj.damage);
	}

	private void OnWaveStart(WaveStartedEvent obj)
	{
		lastEnemySpawned = false;
	}

	private void OnLastEnemySpawned(LastEnemyInWaveSpawnedEvent obj)
	{
		lastEnemySpawned = true;
	}

	private void OnEnemyDeath(EnemyDeathEvent obj)
	{
		enemies.Remove(obj.enemy);
		if (!lastEnemySpawned) return;
		if (enemies.Count == 0) DispatchWaveFinishedEvent();
	}

	private void OnEnemyReachedDestination(EnemyReachedDestinationEvent obj)
	{
		enemies.Remove(obj.enemy);
		if (!lastEnemySpawned) return;
		if (enemies.Count == 0) DispatchWaveFinishedEvent();
	}

	private void OnEnemySpawned(EnemySpawnedEvent obj)
	{
		enemies.Add(obj.enemy);
	}

	private void OnEnemyDamageRequested(EnemyDamageRequestEvent obj)
	{
		obj.enemy.Damage(obj.damageAmount);
		if (obj.elements.Contains(Element.Ice)) obj.enemy.SlowEnemy(0.75f, 5);
	}

	private void DamageEnemies(bool all, int amount, float damage)
	{
		if (all)
		{
			foreach (EnemyController enemy in enemies)
			{
				if (enemy == null) continue;
				enemy.Damage(damage);
			}
		}
		else
		{
			int enemiesDamaged = 0;
			if (enemies.Count < amount) amount = enemies.Count;
			while (enemiesDamaged < amount)
			{
				enemiesDamaged++;
				int randomEnemyIndex = UnityEngine.Random.Range(0, enemies.Count);
				while (enemies[randomEnemyIndex] == null) randomEnemyIndex = UnityEngine.Random.Range(0, enemies.Count);
				enemies[randomEnemyIndex].Damage(damage);
			}
		}
	}

	private void DispatchWaveFinishedEvent()
	{
		CodeControl.Message.Send(new WaveFinishedEvent());
	}
}
