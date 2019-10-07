public class TowerModifierAppliedEvent : CodeControl.Message
{
	public float duration;
	public PropertyModifier[] propertyModifiers;

	public TowerModifierAppliedEvent(float duration, PropertyModifier[] propertyModifiers)
	{
		this.duration = duration;
		this.propertyModifiers = propertyModifiers;
	}
}
