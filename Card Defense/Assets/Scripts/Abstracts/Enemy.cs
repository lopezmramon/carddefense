using UnityEngine;
[System.Serializable]
public class Enemy
{
	public EnemyType enemyType;
	public EnemySpecialAbility specialAbility;
	public int waveIndex, totalWaveEnemies;
	public Transform transform;
	public float maxHealth, currentHealth, speed;
	public System.Action OnHealthChanged;

	public Enemy(EnemyType enemyType, EnemySpecialAbility specialAbility)
	{
		this.enemyType = enemyType;
		this.specialAbility = specialAbility;
	}

	public int Reward
	{
		get
		{
			return (((int)enemyType + 1) * ((int)specialAbility + 1) * 10);
		}
	}

	public int LivesCost
	{
		get
		{
			return 1;
		}
	}
}
