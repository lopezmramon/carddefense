using UnityEngine;

public class ElementalMuzzleParticleRequestEvent : CodeControl.Message
{
	public Element element;
	public Transform location;
	public float firingFrequency;

	public ElementalMuzzleParticleRequestEvent(Element element, Transform location, float firingFrequency)
	{
		this.element = element;
		this.location = location;
		this.firingFrequency = firingFrequency;
	}
}
