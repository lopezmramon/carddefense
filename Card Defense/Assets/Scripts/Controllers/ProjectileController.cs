using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
	private ProjectileMover projectileMover;
	public float damage;

	public void Initialize(Transform target, float speed)
	{
		projectileMover = new ProjectileMover(transform, target, speed);		
	}

	public void Initialize(Vector3 target, float speed)
	{
		projectileMover = new ProjectileMover(transform, target, speed);		
	}

	private void Update()
	{
		projectileMover.MoveStraightToAssignedTarget((success) =>
		{
			if (success)
			{
				projectileMover.target.GetComponent<EnemyController>().Damage(damage);
				Destroy(gameObject);
			}
			else
			{
				Destroy(gameObject);
			}
		});
	}
}
