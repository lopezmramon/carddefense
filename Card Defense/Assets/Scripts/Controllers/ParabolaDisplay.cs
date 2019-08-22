using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolaDisplay : MonoBehaviour
{
	public int numberOfBounces;
	public float initialXDistance, arcHeight;
	public Vector3 startPos, targetPos, nextPos;
	public float speed;
	private bool parable;

	private void Start()
	{
		startPos = transform.position;
		initialXDistance = targetPos.x - startPos.x;
	}

	void Update()
	{
		if (!parable) return;
		// Compute the next position, with arc added in
		float x0 = startPos.x;
		float x1 = targetPos.x;
		float dist = x1 - x0;
		float nextX = Mathf.MoveTowards(transform.position.x, x1, speed * Time.deltaTime);
		float baseY = Mathf.Lerp(startPos.y, targetPos.y, (nextX - x0) / dist);
		float arc = arcHeight * (nextX - x0) * (nextX - x1) / (-0.25f * dist * dist);
		nextPos = new Vector3(nextX, baseY + arc, transform.position.z);

		// Rotate to face the next position, and then move there
		transform.rotation = LookAt2D(nextPos - transform.position);
		transform.position = nextPos;

		// Do something when we reach the target
		if (nextPos == targetPos)
		{
			Arrived();
		}
	}

	void Arrived()
	{
		if (numberOfBounces > 0)
		{
			numberOfBounces--;
			startPos = targetPos;
			targetPos.x += initialXDistance;
			targetPos.y = transform.position.y;
		}
		else Destroy(gameObject);
	}

	private void OnMouseDown()
	{
		parable = true;
	}

	private void OnMouseEnter()
	{
		CodeControl.Message.Send(new DisplayParabolaRequestEvent(transform.position, targetPos, speed, arcHeight, 0.95f, numberOfBounces));
	}

	private void OnMouseExit()
	{
	//	CodeControl.Message.Send(new StopDisplayingParabolaRequestEvent());
	}
	/// 
	/// This is a 2D version of Quaternion.LookAt; it returns a quaternion
	/// that makes the local +X axis point in the given forward direction.
	/// 
	/// forward direction
	/// Quaternion that rotates +X to align with forward
	public static Quaternion LookAt2D(Vector2 forward)
	{
		return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);
	}
}
