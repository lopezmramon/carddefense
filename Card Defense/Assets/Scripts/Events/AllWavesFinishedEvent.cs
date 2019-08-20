
public class AllWavesFinishedEvent : CodeControl.Message
{
	public int totalWaves, totalEnemies;

	public AllWavesFinishedEvent(int totalWaves, int totalEnemies)
	{
		this.totalWaves = totalWaves;
		this.totalEnemies = totalEnemies;
	}
}
