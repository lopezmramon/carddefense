public class ChangeViewRequestEvent : CodeControl.Message
{
	public View previousView, requestedView;
	public bool togglePreviousOff;

	public ChangeViewRequestEvent(View previousView, View requestedView, bool togglePreviousOff)
	{
		this.previousView = previousView;
		this.requestedView = requestedView;
		this.togglePreviousOff = togglePreviousOff;
	}
}
