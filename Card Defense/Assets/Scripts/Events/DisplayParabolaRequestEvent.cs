using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayParabolaRequestEvent : CodeControl.Message
{
	public Transform projectileOrigin;
	public Vector3 target;
	public float speed;
	public float arcHeight;
	public float xReductionPerBounce;
	public int bounces;
	public Transform variableTarget;

	public DisplayParabolaRequestEvent(Transform projectileOrigin, Vector3 target, float speed, float arcHeight, float xReductionPerBounce, int bounces)
	{
		this.projectileOrigin = projectileOrigin;
		this.target = target;
		this.speed = speed;
		this.arcHeight = arcHeight;
		this.xReductionPerBounce = xReductionPerBounce;
		this.bounces = bounces;
	}

	public DisplayParabolaRequestEvent(Transform projectileOrigin, float speed, float arcHeight, float xReductionPerBounce, int bounces, Transform variableTarget)
	{
		this.projectileOrigin = projectileOrigin;
		this.speed = speed;
		this.arcHeight = arcHeight;
		this.xReductionPerBounce = xReductionPerBounce;
		this.bounces = bounces;
		this.variableTarget = variableTarget;
	}
}
