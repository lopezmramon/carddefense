[System.Serializable]
public class Enemy 
{
	public EnemyType enemyType;
	public EnemySpecialAbility specialAbility;
	public int livesCost;
	
	public Enemy(EnemyType enemyType, EnemySpecialAbility specialAbility)
	{
		this.enemyType = enemyType;
		this.specialAbility = specialAbility;
	}
}
