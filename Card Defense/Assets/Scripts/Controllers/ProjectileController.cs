using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
	private ProjectileMover projectileMover;
	public float damage, aoe;
	private int reflectionsLeft;
	private ProjectileMovementType primaryMovementType;

	public void InitializeStraightChainProjectile(Transform target, int chainLength, float speed, float aoe, ProjectileMovementType primaryMovementType)
	{
		reflectionsLeft = chainLength;
		this.aoe = aoe;
		this.primaryMovementType = primaryMovementType;
		projectileMover = new ProjectileMover(transform, target, speed);		
	}

	public void InitializeBounceProjectile(Vector3 target, int bounces, float speed, float aoe, ProjectileMovementType primaryMovementType)
	{
		reflectionsLeft = bounces;
		this.primaryMovementType = primaryMovementType;
		this.aoe = aoe;
		projectileMover = new ProjectileMover(transform, target, speed);		
	}

	private void Update()
	{
		if (projectileMover == null) return;
		switch (primaryMovementType)
		{
			case ProjectileMovementType.StraightChain:
				projectileMover.MoveStraightToAssignedTarget((success)=> 
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
				}
				);
				break;
			case ProjectileMovementType.BounceOnGround:
				projectileMover.BounceOnTargetPosition((success)=> 
				{
					if (success)
					{
						AnalyzeBounceResult();
					}
					else
					{
						Destroy(gameObject);
					}
				});
				break;
		}
	}

	private void AnalyzeBounceResult()
	{
		reflectionsLeft--;
		Collider[] hitEnemies = Physics.OverlapSphere(transform.position, aoe, LayerMask.NameToLayer("Enemy"));
		foreach(Collider collider in hitEnemies)
		{
			if(collider.GetComponent<EnemyController>() != null)
			{
				collider.GetComponent<EnemyController>().Damage(damage);
			}
		}
		if (reflectionsLeft <= 0) Destroy(gameObject);
	}
}
