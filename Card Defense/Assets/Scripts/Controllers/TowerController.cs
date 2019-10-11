using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerController : MonoBehaviour, IPointerDownHandler
{
	private Tower tower;
	public Tower GetTower { get { return tower; } }
	private float progress;
	public float fireRate, fireCountdown, fireRange, rotationSpeed = 10f, buildTime, lerpElapsedTime;
	public Material[] buildMaterials;
	private Material finalMaterial;
	public Renderer[] renderers;
	public MeshRenderer TowerBaseRenderer => Array.Find(renderers, (x) => x.name.Contains("Base")) as MeshRenderer;
	public MeshRenderer TowerTurretRenderer => Array.Find(renderers, (x) => x.name.Contains("Turret") || x.name.Contains("Crystal")) as MeshRenderer;
	public MeshRenderer TowerBarrelRenderer => Array.Find(renderers, (x) => x.name.Contains("Barrel") || x.name.Contains("Crystal")) as MeshRenderer;
	public Transform projectileOrigin, rotatingPart, target, chargeRotationCenter;
	public TowerTargetController targetController;
	public TowerTargetController targetControllerPrefab;
	public Queue<Element> elements = new Queue<Element>();
	private TowerTargeting towerTargeting;
	private Animator animator;
	private bool building = false;
	public PropertyModifierHandler propertyModifierHandler;
	private bool canFire;
	public bool ToggleShooting
	{
		get
		{
			return canFire;
		}
		set
		{
			canFire = value;
		}
	}

	private void Awake()
	{
		animator = GetComponent<Animator>();
		propertyModifierHandler = new PropertyModifierHandler((particle) => Destroy(particle));
	}

	private void SetupTargeting()
	{
		towerTargeting = ElementUtility.TowerTargetingForBaseElement(elements.Peek());
		if (towerTargeting == TowerTargeting.Ground) GenerateTargetObject();
	}

	private void GenerateTargetObject()
	{
		targetController = Instantiate(targetControllerPrefab);
		Vector3 initialTargetPosition = projectileOrigin.transform.position;
		initialTargetPosition.y = 0;
		targetController.Initialize(initialTargetPosition, elements.ToArray());
	}

	public void DeactivateTarget()
	{
		if (targetController != null) targetController.Deactivate();
	}

	public void ActivateTarget()
	{
		if (targetController != null) targetController.Activate();
	}

	private void Update()
	{
		if (building)
		{
			Build();
			return;
		}
		LookAtTarget();
		if (!canFire) return;
		if (target == null || Vector3.Distance(transform.position, target.position) > fireRange)
		{
			DetectEnemies();
		}
		ShootingCountdown();
		propertyModifierHandler.PropertyCountdowns();
	}

	private void ShootingCountdown()
	{
		if (!canFire) return;
		if (fireCountdown >= 0)
		{
			fireCountdown -= Time.deltaTime * GameManager.gameSpeedMultiplier;
		}
		else
		{
			fireCountdown = 1f / (fireRate * propertyModifierHandler.fireRateMultiplier);
			FireProjectileAtTarget();
		}
	}

	private void FireProjectileAtTarget()
	{
		if (building) return;
		switch (towerTargeting)
		{
			case TowerTargeting.Direct:
				if (target == null) return;
				DispatchShootProjectileRequestEvent(target);
				break;
			case TowerTargeting.Ground:
				DispatchShootProjectileRequestEvent(targetController.transform);
				break;
			case TowerTargeting.NoTarget:
				DispatchShootProjectileRequestEvent(null);
				break;
		}
		animator.SetTrigger("Shoot");
	}

	private void Build()
	{
		if (lerpElapsedTime < buildTime)
		{
			progress = Mathf.Lerp(-1f, 1f, lerpElapsedTime / buildTime);
			foreach (Renderer renderer in renderers)
			{
				renderer.material.SetFloat("_EffectProgress", progress);
				if (progress > 0.45f)
				{
					float edgeWidth = renderer.material.GetFloat("_EdgeWidth");
					if (edgeWidth > 0) edgeWidth -= Time.deltaTime * GameManager.gameSpeedMultiplier;
					renderer.material.SetFloat("_EdgeWidth", edgeWidth);
				}
			}
			lerpElapsedTime += Time.deltaTime * GameManager.gameSpeedMultiplier;
		}
		else
		{
			foreach (Renderer renderer in renderers)
			{
				renderer.material = finalMaterial;
			}
			building = false;
		}
	}

	private void ResetRotation()
	{
		Quaternion rotation = Quaternion.identity;
		rotatingPart.rotation = Quaternion.Slerp(rotatingPart.rotation, rotation, Time.deltaTime * rotationSpeed);
	}

	private void LookAtTarget()
	{
		Vector3 targetLocation = Vector3.one;
		if (towerTargeting == TowerTargeting.Direct)
		{
			if (target == null) return;
			targetLocation = target.transform.position;
		}
		else if (towerTargeting == TowerTargeting.Ground)
		{
			targetLocation = targetController.transform.position;
			targetLocation.y += 0.5f;
		}
		Quaternion rotation = Quaternion.LookRotation(targetLocation - rotatingPart.position);
		rotatingPart.rotation =
		Quaternion.RotateTowards(rotatingPart.rotation,
		rotation, Time.deltaTime * rotationSpeed * GameManager.gameSpeedMultiplier);
	}

	private void DetectEnemies()
	{
		if (towerTargeting == TowerTargeting.Ground) return;
		foreach (Collider collider in Physics.OverlapSphere(transform.position, fireRange, 1 << LayerMask.NameToLayer("Enemy")))
		{
			if (collider != null)
			{
				RepositionTargetDependingOnTargeting(collider.transform);
			}
			else
			{
				ResetTargetDependingOnTargeting();
			}
		}
	}

	private void RepositionTargetDependingOnTargeting(Transform enemyToTarget)
	{
		switch (towerTargeting)
		{
			case TowerTargeting.Direct:
				target = enemyToTarget;
				break;
			case TowerTargeting.Ground:
				break;
			case TowerTargeting.NoTarget:
				break;
		}
	}

	private void ResetTargetDependingOnTargeting()
	{
		switch (towerTargeting)
		{
			case TowerTargeting.Direct:
				target = null;
				break;
			case TowerTargeting.Ground:
				//Vector3 targetPos = targetController.transform.position;
				//	targetPos.y = projectileOrigin.position.y;
				//	targetController.transform.position = targetPos;
				break;
			case TowerTargeting.NoTarget:
				break;
		}
	}

	public void Initialize(float buildTime, Element initialElement, bool canFire)
	{
		this.canFire = canFire;
		Build(buildTime, initialElement);
		SetupTargeting();
	}

	private void Build(float time, Element element)
	{
		elements.Enqueue(element);
		StartBuilding(time);
	}

	private void StartBuilding(float time)
	{
		progress = -1f;
		lerpElapsedTime = 0;
		buildTime = time;
		Material dissolveControlMaterial = Instantiate(buildMaterials[0]);
		finalMaterial = renderers[0].material;
		foreach (Renderer renderer in renderers)
		{
			renderer.material = dissolveControlMaterial;
		}
		building = true;
	}

	internal void Upgrade(Element element)
	{
		elements.Enqueue(element);
		DispatchTowerUpgradedEvent();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			if (towerTargeting == TowerTargeting.Ground)
			{
				DispatchShowTargetRequestEvent();
				DispatchDisplayParabolaRequestEvent();
			}
			DispatchTowerInfoUIDisplayRequestEvent();
		}
		else if (eventData.button == PointerEventData.InputButton.Right)
		{
			//DispatchSellTowerRequestEvent();
		}
	}

	private void DispatchTowerInfoUIDisplayRequestEvent()
	{
		CodeControl.Message.Send(new TowerInfoUIDisplayRequestEvent(this));
	}

	private void DispatchSellTowerRequestEvent()
	{
		CodeControl.Message.Send(new SellTowerRequestEvent(this));
	}

	private void DispatchShowTargetRequestEvent()
	{
		CodeControl.Message.Send(new ShowTargetRequestEvent(this));
	}

	private void DispatchDisplayParabolaRequestEvent()
	{
		CodeControl.Message.Send(new DisplayParabolaRequestEvent(
			projectileOrigin,
			ElementUtility.SpeedFromElements(elements.ToArray()),
			projectileOrigin.position.y, 0,
			ElementUtility.BouncesFromElements(elements.ToArray()),
			targetController.transform));
	}

	private void DispatchStopParabolaDisplayRequestEvent()
	{
		CodeControl.Message.Send(new StopDisplayingParabolaRequestEvent());
	}

	private void DispatchTowerUpgradedEvent()
	{
		CodeControl.Message.Send(new TowerUpgradedEvent());
	}

	private void DispatchShootProjectileRequestEvent(Transform target)
	{
		Transform projectileTarget = towerTargeting == TowerTargeting.Direct ? target : towerTargeting == TowerTargeting.Ground ? targetController.transform : null;
		Projectile projectile = new Projectile(propertyModifierHandler.projectileDamageMultiplier, propertyModifierHandler.rangeMultiplier, propertyModifierHandler.projectileAOEMultiplier, elements, target);
		CodeControl.Message.Send(new ShootProjectileRequestEvent(projectile, projectileOrigin));
		if (towerTargeting != TowerTargeting.NoTarget)
		{
			DispatchElementalMuzzleParticleRequestEvent();
		}
	}

	private void DispatchElementalMuzzleParticleRequestEvent()
	{
		CodeControl.Message.Send(new ElementalMuzzleParticleRequestEvent(elements.Peek(), projectileOrigin, fireRate * propertyModifierHandler.fireRateMultiplier));
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, fireRange);
	}
}
