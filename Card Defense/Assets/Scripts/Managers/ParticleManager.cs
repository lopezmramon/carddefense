using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ParticleManager : MonoBehaviour
{
	public GameObject[] elementalChargeParticlePrefabs,
		elementalContactParticlePrefabs,
		elementalMuzzleParticlePrefabs,
		propertyModifierParticlePrefabs;
	private List<GameObject> temporaryParticles = new List<GameObject>();
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
		CodeControl.Message.AddListener<SimulateTowerModifierRequestEvent>(OnTowerModifierSimulateRequested);
		CodeControl.Message.AddListener<TowerModifierAppliedEvent>(OnTowerModifierApplied);
	}

	private void OnTowerModifierApplied(TowerModifierAppliedEvent obj)
	{
		DestroyTemporaryParticlesAfterDuration(obj.duration);
	}

	private void OnTowerModifierSimulateRequested(SimulateTowerModifierRequestEvent obj)
	{
		PlaceModifierParticle(obj.tower, obj.propertyModifiers);
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
		DestroyTemporaryParticles();
	}

	private void OnTowerSold(TowerSoldEvent obj)
	{
		orbitGroups.Remove(obj.tower);
	}

	private void OnTowerUpgraded(TowerUpgradedEvent obj)
	{
		temporaryParticles.Clear();
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

	private void PlaceModifierParticle(TowerController tower, PropertyModifier[] propertyModifiers)
	{
		if (propertyModifiers[0] == PropertyModifier.None) return;
		GameObject temporaryParticle = Instantiate(propertyModifierParticlePrefabs[(int)propertyModifiers[0]], tower.transform);
		temporaryParticles.Add(temporaryParticle);
		ParticleSystem ps = temporaryParticle.GetComponent<ParticleSystem>();
		ShapeModule shape = ps.shape;
		MeshRenderer targetMesh = PropertyModifierHelper.FindCorrectMeshRendererForProperty(tower, propertyModifiers);			
		shape.meshRenderer = targetMesh;
	}


	private void GenerateElementalCharge(TowerController tower, Element element)
	{
		if (!orbitGroups.ContainsKey(tower))
		{
			orbitGroups.Add(tower, new OrbitGroup());
		}
		ChargeParticleController chargeParticleController = Instantiate(elementalChargeParticlePrefabs[(int)element]).GetComponent<ChargeParticleController>();
		temporaryParticles.Add(chargeParticleController.gameObject);
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

	private void DestroyTemporaryParticlesAfterDuration(float duration)
	{
		foreach (GameObject particle in temporaryParticles)
		{
			Destroy(particle, duration);
		}
		temporaryParticles.Clear();
	}

	private void DestroyTemporaryParticles()
	{
		DestroyTemporaryParticlesAfterDuration(0);
	}
}
