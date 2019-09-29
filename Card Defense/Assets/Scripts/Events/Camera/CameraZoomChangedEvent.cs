public class CameraZoomChangedEvent : CodeControl.Message
{
	public float zoomLevel;

	public CameraZoomChangedEvent(float zoomLevel)
	{
		this.zoomLevel = zoomLevel;
	}
}
