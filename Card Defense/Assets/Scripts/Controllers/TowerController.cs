using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
	public float fireRate, fireCountdown, fireRange;
	public ProjectileController projectile;
	public Transform projectileOrigin;
	public List<Element> elements = new List<Element>();

	private void Update()
	{
		if (fireCountdown >= 0)
		{
			fireCountdown -= Time.deltaTime;
		}
		else
		{
			fireCountdown = 1f/ fireRate;
			DetectEnemies();
		}
	}

	private void DetectEnemies()
	{
		foreach (Collider collider in Physics.OverlapSphere(transform.position, fireRange, LayerMask.NameToLayer("Enemy")))
		{
			if (collider != null && collider.CompareTag("Enemy"))
			{
				FireProjectileAtTarget(collider.transform);				
				return;
			}
		}
	}

	private void FireProjectileAtTarget(Transform target)
	{
		ProjectileController projectileShot = Instantiate(projectile);
		projectileShot.transform.position = projectileOrigin.position;
		projectileShot.Initialize(target, elements);
	}

	internal void SimulateUpgrade(Element element)
	{
		throw new NotImplementedException();
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, fireRange);	
	}

	internal void Upgrade(Element element)
	{
		throw new NotImplementedException();
	}
}
