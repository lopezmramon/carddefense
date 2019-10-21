using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ProjectileManager : MonoBehaviour
{
	public ProjectileCollection projectileCollection;
	public GroundHazardController groundHazardPrefab;

	private void Awake()
	{
		CodeControl.Message.AddListener<ShootProjectileRequestEvent>(OnShootProjectileRequested);
		CodeControl.Message.AddListener<GroundHazardPlacementRequestEvent>(OnGroundHazardPlacementRequested);
	}

	private void OnGroundHazardPlacementRequested(GroundHazardPlacementRequestEvent obj)
	{
		PlaceGroundHazard(obj.projectile, obj.location);
	}

	private void OnShootProjectileRequested(ShootProjectileRequestEvent obj)
	{
		ShootProjectile(obj.projectile, obj.projectileOrigin);
	}

	private void ShootProjectile(Projectile projectile, Transform projectileOrigin)
	{
		ProjectileController projectileShot = Instantiate(projectileCollection.GetAttribute(projectile.elements.Peek()));
		projectileShot.transform.position = projectileOrigin.position;
		projectileShot.Initialize(projectile);
	}

	private void PlaceGroundHazard(Projectile projectile, Vector3 location)
	{
		GroundHazardController hazard = Instantiate(groundHazardPrefab);
		hazard.transform.position = location;
		hazard.Initialize(projectile);
	}
}
