using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayParabolaRequestEvent : CodeControl.Message
{
	public Vector3 startingPos;
	public Vector3 target;
	public float speed;
	public float arcHeight;
	public float xReductionPerBounce;
	public int bounces;

	public DisplayParabolaRequestEvent(Vector3 startingPos, Vector3 target, float speed, float arcHeight, float xReductionPerBounce, int bounces)
	{
		this.startingPos = startingPos;
		this.target = target;
		this.speed = speed;
		this.arcHeight = arcHeight;
		this.xReductionPerBounce = xReductionPerBounce;
		this.bounces = bounces;
	}
}
