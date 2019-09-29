public class SimulateUpgradeRequestEvent : CodeControl.Message
{
	public TowerController tower;
	public Element element;

	public SimulateUpgradeRequestEvent(TowerController tower, Element element)
	{
		this.tower = tower;
		this.element = element;
	}
}
