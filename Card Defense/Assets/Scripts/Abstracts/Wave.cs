using System.Collections.Generic;
[System.Serializable]
public class Wave
{
	public List<Enemy> enemies;

	public Wave()
	{
		enemies = new List<Enemy>();
	}
}
