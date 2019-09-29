using UnityEngine;

public class GroundHazardPlacementRequestEvent : CodeControl.Message
{
	public Projectile projectile;
	public Vector3 location;

	public GroundHazardPlacementRequestEvent(Projectile projectile, Vector3 location)
	{
		this.projectile = projectile;
		this.location = location;
	}
}
