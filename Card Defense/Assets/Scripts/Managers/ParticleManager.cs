using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
	public GameObject[] elementalChargeParticlePrefabs,
		elementalContactParticlePrefabs,
		elementalMuzzleParticlePrefabs;
	private GameObject temporaryChargeParticle;
	private Dictionary<TowerController, OrbitGroup> orbitGroups = new Dictionary<TowerController, OrbitGroup>();
	[Header("Elemental Charge Properties")]
	public float radius;
	public float radiusSpeed, rotationSpeed;

	private void Awake()
	{
		CodeControl.Message.AddListener<SimulateUpgradeRequestEvent>(OnUpgradeSimulateRequested);
		CodeControl.Message.AddListener<CardNoLongerOverTileEvent>(OnCardNoLongerOverTile);
		CodeControl.Message.AddListener<TowerUpgradedEvent>(OnTowerUpgraded);
		CodeControl.Message.AddListener<TowerSoldEvent>(OnTowerSold);
		CodeControl.Message.AddListener<ElementalContactParticleRequestEvent>(OnElementalContactParticleRequested);
		CodeControl.Message.AddListener<ElementalMuzzleParticleRequestEvent>(OnElementalMuzzleParticleRequested);
	}

	private void OnElementalMuzzleParticleRequested(ElementalMuzzleParticleRequestEvent obj)
	{
		PlaceMuzzleParticle(obj.element, obj.location, obj.firingFrequency);
	}

	private void OnElementalContactParticleRequested(ElementalContactParticleRequestEvent obj)
	{
		PlaceElementalContactParticle(obj.element, obj.placements);
	}

	private void OnCardNoLongerOverTile(CardNoLongerOverTileEvent obj)
	{
		if (temporaryChargeParticle != null) Destroy(temporaryChargeParticle);
	}

	private void OnTowerSold(TowerSoldEvent obj)
	{
		orbitGroups.Remove(obj.tower);
	}

	private void OnTowerUpgraded(TowerUpgradedEvent obj)
	{
		temporaryChargeParticle = null;
	}

	private void OnUpgradeSimulateRequested(SimulateUpgradeRequestEvent obj)
	{
		GenerateElementalCharge(obj.tower, obj.element);
	}

	private void PlaceElementalContactParticle(Element element, Vector3[] placements)
	{
		foreach (Vector3 placement in placements)
		{
			GameObject particle = Instantiate(elementalContactParticlePrefabs[(int)element]);
			particle.transform.position = placement;
			Destroy(particle, 2);
		}
	}

	private void GenerateElementalCharge(TowerController tower, Element element)
	{
		if (!orbitGroups.ContainsKey(tower))
		{
			orbitGroups.Add(tower, new OrbitGroup());
		}
		ChargeParticleController chargeParticleController = Instantiate(elementalChargeParticlePrefabs[(int)element]).GetComponent<ChargeParticleController>();
		temporaryChargeParticle = chargeParticleController.gameObject;
		OrbitMovement particleOrbit = orbitGroups[tower].AddOrbitMovement(
			chargeParticleController.transform, tower.chargeRotationCenter,
			radius, radiusSpeed, rotationSpeed, tower.elements.Peek());
		chargeParticleController.Initialize(tower.chargeRotationCenter, particleOrbit);
	}

	private void PlaceMuzzleParticle(Element element, Transform location, float firingFrequency)
	{
		MuzzleParticleController muzzleParticleController = Instantiate(elementalMuzzleParticlePrefabs[(int)element], location).GetComponent<MuzzleParticleController>();
		muzzleParticleController.Initialize(firingFrequency);
	}

}
