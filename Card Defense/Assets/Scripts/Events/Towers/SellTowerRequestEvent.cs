public class SellTowerRequestEvent : CodeControl.Message
{
	public TowerController towerController;

	public SellTowerRequestEvent(TowerController towerController)
	{
		this.towerController = towerController;
	}
}
