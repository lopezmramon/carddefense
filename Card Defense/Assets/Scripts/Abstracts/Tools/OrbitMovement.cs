using UnityEngine;

[System.Serializable]
public class OrbitMovement
{
	GameObject cube;
	public Transform center, transform;
	public Vector3 axis;
	public Vector3 desiredPosition;
	public float radius = 2.0f;
	public float radiusSpeed = 0.5f;
	public float rotationSpeed = 80.0f;

	public OrbitMovement(Transform transform, Transform center, Vector3 axis,
		float radius, float radiusSpeed, float rotationSpeed)
	{
		this.transform = transform;
		this.center = center;
		this.radius = radius;
		this.axis = axis;
		this.radiusSpeed = radiusSpeed;
		this.rotationSpeed = rotationSpeed;
		transform.position = (transform.position - center.position).normalized * radius + center.position;
		radius = 2.0f;
	}

	public void Rotate()
	{
		if (center == null || transform == null) return;
		transform.RotateAround(center.position, axis, rotationSpeed * Time.deltaTime * GameManager.gameSpeedMultiplier);
		desiredPosition = (transform.position - center.position).normalized * radius + center.position;
		transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * radiusSpeed * GameManager.gameSpeedMultiplier);
	}
}
