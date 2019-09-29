public class ShowTargetRequestEvent : CodeControl.Message
{
	public TowerController towerController;

	public ShowTargetRequestEvent(TowerController towerController)
	{
		this.towerController = towerController;
	}
}
