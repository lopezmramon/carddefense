using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerController : MonoBehaviour, IPointerDownHandler
{
	public float fireRate, fireCountdown, fireRange, rotationSpeed = 10f;
	public ProjectileController projectile;
	public Transform projectileOrigin, rotatingPart, target;
	public List<Element> elements = new List<Element>();
	private ProjectileMovementType projectileMovementType;
	private Animator animator;

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}

	private void Start()
	{
		projectileMovementType = ElementUtility.MovementForElement(elements[0]);
	}

	private void Update()
	{
		if (target == null || Vector3.Distance(transform.position, target.position) > fireRange)
		{
			DetectEnemies();
		}
		if (target == null)
		{
			//ResetRotation();
			return;
		}
		LookAtTarget();
		if (fireCountdown >= 0)
		{
			fireCountdown -= Time.deltaTime;
		}
		else
		{
			fireCountdown = 1f / fireRate;
			FireProjectileAtTarget(target);
		}
	}

	private void ResetRotation()
	{
		Quaternion rotation = Quaternion.identity;
		// rotation.x = 0; This is for limiting the rotation to the y axis. I needed this for my project so just
		// rotation.z = 0;                 delete or add the lines you need to have it behave the way you want.
		rotatingPart.rotation = Quaternion.Slerp(rotatingPart.rotation, rotation, Time.deltaTime * rotationSpeed);
	}

	private void LookAtTarget()
	{
		Quaternion rotation = Quaternion.LookRotation(target.position - rotatingPart.position);
		// rotation.x = 0; This is for limiting the rotation to the y axis. I needed this for my project so just
		// rotation.z = 0;                 delete or add the lines you need to have it behave the way you want.
		//rotatingPart.rotation = Quaternion.Slerp(rotatingPart.rotation, rotation, Time.deltaTime * rotationSpeed);
		rotatingPart.rotation =
		Quaternion.RotateTowards(rotatingPart.rotation, // start from our current orientation
		rotation, // blend toward the target rotation
		Time.deltaTime * rotationSpeed // turn at most rotationSpeed degrees per second
		);
	}

	private void DetectEnemies()
	{
		foreach (Collider collider in Physics.OverlapSphere(transform.position, fireRange, LayerMask.NameToLayer("Enemy")))
		{
			if (collider != null && collider.CompareTag("Enemy"))
			{
				target = collider.transform;
				return;
			}
			else
			{
				target = null;
			}
		}
	}

	private void FireProjectileAtTarget(Transform target)
	{
		ProjectileController projectileShot = Instantiate(projectile);
		animator.SetTrigger("Shoot");
		projectileShot.transform.position = projectileOrigin.position;
		ProjectileMovementType primaryMovementType = ElementUtility.MovementForElement(elements[0]);
		switch (ElementUtility.MovementForElement(elements[0]))
		{
			case ProjectileMovementType.StraightChain:
				projectileShot.InitializeStraightChainProjectile(target, BouncesFromElements(), SpeedFromElements(), LandingAOEFromElements(), primaryMovementType);
				break;
			case ProjectileMovementType.BounceOnGround:
				projectileShot.InitializeBounceProjectile(target.position, BouncesFromElements(), SpeedFromElements(), LandingAOEFromElements(), primaryMovementType);
				break;
			case ProjectileMovementType.AOEAtTower:
				break;
		}
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

	public void OnPointerDown(PointerEventData eventData)
	{
		throw new NotImplementedException();
	}

	private void DispatchParabolaDisplayRequestEvent()
	{
		if (ElementUtility.MovementForElement(elements[0]) == ProjectileMovementType.BounceOnGround)
		{

		}
	}

	private float SpeedFromElements()
	{
		return 8f;
	}

	private float LandingAOEFromElements()
	{
		return 3f;
	}

	private int BouncesFromElements()
	{
		return 3;
	}
}
