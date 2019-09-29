using UnityEngine;

public class ElementalContactParticleRequestEvent : CodeControl.Message
{
	public Element element;
	public Vector3[] placements;

	public ElementalContactParticleRequestEvent(Element element, Vector3 placement)
	{
		this.element = element;
		placements = new Vector3[] { placement };
	}

	public ElementalContactParticleRequestEvent(Element element, Vector3[] placements)
	{
		this.element = element;
		this.placements = placements;
	}
}
