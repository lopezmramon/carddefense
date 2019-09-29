public class CameraZoomChangeRequestEvent : CodeControl.Message
{
	public bool increase;

	public CameraZoomChangeRequestEvent(bool increase)
	{
		this.increase = increase;
	}
}
