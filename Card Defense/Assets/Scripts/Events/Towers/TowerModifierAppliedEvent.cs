public class TowerModifierAppliedEvent : CodeControl.Message
{
	public PropertyModifierHandler handler;
	public PropertyModifier[] propertyModifiers;

	public TowerModifierAppliedEvent(PropertyModifierHandler handler, PropertyModifier[] propertyModifiers)
	{
		this.handler = handler;
		this.propertyModifiers = propertyModifiers;
	}
}
