using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
	public TowerController[] baseTowers, visualTowers;
	public static Dictionary<Tile, TowerController> towersPlaced = new Dictionary<Tile, TowerController>();
	private CardContainer cardOverTile;
	private Tile hoverTile;

	private void Awake()
	{
		CodeControl.Message.AddListener<TowerPlacementRequestEvent>(OnTowerPlacementRequested);
		CodeControl.Message.AddListener<TowerRemovedEvent>(OnTowerRemoved);
		CodeControl.Message.AddListener<CardOverTileEvent>(OnCardOverTile);
		CodeControl.Message.AddListener<CardNoLongerOverTileEvent>(OnCardNoLongerOverTile);
		CodeControl.Message.AddListener<CardDroppedEvent>(OnCardDropped);
		CodeControl.Message.AddListener<ShowTargetRequestEvent>(OnTargetShowRequested);
		CodeControl.Message.AddListener<StopShowingTargetsRequestEvent>(OnStopShowingTargetsRequested);
		CodeControl.Message.AddListener<SellTowerRequestEvent>(OnSellTowerRequested);
		GenerateVisualTowers();
	}

	private void OnSellTowerRequested(SellTowerRequestEvent obj)
	{
		SellTower(obj.towerController);
	}

	private void OnStopShowingTargetsRequested(StopShowingTargetsRequestEvent obj)
	{
		DeactivateAllTargets();
		DispatchStopDisplayingParabolaRequestEvent();
	}

	private void OnTargetShowRequested(ShowTargetRequestEvent obj)
	{
		DeactivateAllTargets();
		obj.towerController.ActivateTarget();
	}

	private void SellTower(TowerController towerController)
	{
		KeyValuePair<Tile, TowerController> item = towersPlaced.First(kvp => kvp.Value == towerController);
		towersPlaced.Remove(item.Key);
		DispatchTowerSoldEvent(towerController);
		Destroy(towerController.gameObject);
	}

	private void DeactivateAllTargets()
	{
		foreach (KeyValuePair<Tile, TowerController> tower in towersPlaced)
		{
			tower.Value.DeactivateTarget();
		}
	}

	private void OnCardDropped(CardDroppedEvent obj)
	{
		if (hoverTile == null || cardOverTile == null) return;

		if (cardOverTile.card.cardType == CardType.Tower)
		{
			if (ResourceManager.currentResourceAmount >= cardOverTile.card.cost)
			{
				ProcessRequest(hoverTile, cardOverTile.card.element);
				DispatchResourceChangeRequestEvent(-cardOverTile.card.cost);
				DispatchCardConsumeRequestEvent();
			}
			else
			{
				DeactivateAllVisualTowers();
			}
		}
		else if (cardOverTile.card.cardType == CardType.PropertyModifier)
		{
			if (!towersPlaced.ContainsKey(hoverTile)) return;
			if (ResourceManager.currentResourceAmount >= cardOverTile.card.cost)
			{
				AddMultipliers(hoverTile, cardOverTile.card.propertyModifiers, cardOverTile.card.propertyModifierValues, cardOverTile.card.duration);
				DispatchResourceChangeRequestEvent(-cardOverTile.card.cost);
				DispatchCardConsumeRequestEvent();
			}
		}

	}

	private void DispatchCardConsumeRequestEvent()
	{
		CodeControl.Message.Send(new CardsConsumeRequestEvent(cardOverTile));
		cardOverTile = null;
	}

	private void OnCardNoLongerOverTile(CardNoLongerOverTileEvent obj)
	{
		DeactivateAllVisualTowers();
		hoverTile = null;
		cardOverTile = null;
	}

	private void OnCardOverTile(CardOverTileEvent obj)
	{
		hoverTile = obj.tile;
		cardOverTile = obj.card;
		Card card = obj.card.card;
		if (towersPlaced.ContainsKey(obj.tile))
		{
			if (card.cardType == CardType.Tower)
			{
				SimulateUpgrade(towersPlaced[obj.tile], card);
			}
			else if (card.cardType == CardType.PropertyModifier)
			{
				SimulateModifier(towersPlaced[obj.tile], card);
			}
		}
		else
		{
			DispatchTileVFXRequestEvent(obj.tile, card.element);
			if (card.cardType == CardType.Tower) PlaceVisualTowerAtTile(obj.tile, card.element);
		}
	}

	private void AddMultipliers(Tile hoverTile, PropertyModifier[] propertyModifiers, float[] propertyModifierValues, float duration)
	{
		TowerController tower = towersPlaced[hoverTile];
		if (tower == null)
		{
			towersPlaced.Remove(hoverTile);
			return;
		}
		for (int i = 0; i < propertyModifiers.Length; i++)
		{
			switch (propertyModifiers[i])
			{
				case PropertyModifier.Damage:
					tower.propertyModifierHandler.AddProjectileDamageMultiplier(propertyModifierValues[i] / 100, duration);
					break;
				case PropertyModifier.FireRate:
					tower.propertyModifierHandler.AddFireRateMultiplier(propertyModifierValues[i] / 100, duration);
					break;
				case PropertyModifier.AOE:
					tower.propertyModifierHandler.AddAOEMultiplier(propertyModifierValues[i] / 100, duration);
					break;
				case PropertyModifier.Range:
					tower.propertyModifierHandler.AddProjectileSpeedMultiplier(propertyModifierValues[i] / 100, duration);
					break;
			}
		}
		DispatchPropertyModifiersAppliedEvent(propertyModifiers, duration);
	}

	private void SimulateModifier(TowerController towerController, Card card)
	{
		if (card.propertyModifiers[0] != PropertyModifier.None)
		{
			DispatchSimulateTowerModifierRequestEvent(towerController, card.propertyModifiers, card.propertyModifierValues);
		}
	}

	private void SimulateUpgrade(TowerController tower, Card card)
	{
		DispatchSimulateTowerUpgradeRequestEvent(tower, card.element);
	}

	private void PlaceVisualTowerAtTile(Tile tile, Element element)
	{
		TowerController visualTower = visualTowers[(int)element];
		visualTower.gameObject.SetActive(true);
		visualTower.transform.SetParent(tile.transform);
		visualTower.transform.localPosition = new Vector3(0, 0.5f, 0);
	}

	private void OnTowerRemoved(TowerRemovedEvent obj)
	{
		towersPlaced.Remove(obj.tile);
	}

	private void OnTowerPlacementRequested(TowerPlacementRequestEvent obj)
	{
		ProcessRequest(obj.tile, obj.element);
	}

	private void DeactivateAllVisualTowers()
	{
		foreach (TowerController tower in visualTowers)
		{
			if (tower == null) continue;
			tower.gameObject.SetActive(false);
		}
	}

	private void GenerateVisualTowers()
	{
		visualTowers = new TowerController[baseTowers.Length];
		for (int i = 0; i < visualTowers.Length; i++)
		{
			visualTowers[i] = Instantiate(baseTowers[i]);
			visualTowers[i].fireRange = 0;
			visualTowers[i].name = baseTowers[i].name;
			visualTowers[i].gameObject.SetActive(false);
		}
	}

	private void ProcessRequest(Tile tile, Element element)
	{
		if (towersPlaced.ContainsKey(tile))
		{
			UpgradeTower(tile, element);
		}
		else
		{
			BuildBaseTower(tile, element);
		}
	}

	private void BuildBaseTower(Tile tile, Element element)
	{
		if (tile.transform == null) return;
		TowerController tower = Instantiate(baseTowers[(int)element], tile.transform);
		tower.Initialize(2.5f, element);
		tower.transform.localPosition = new Vector3(0, 0.5f, 0);
		towersPlaced.Add(tile, tower);
	}

	private void UpgradeTower(Tile tile, Element element)
	{
		towersPlaced[tile].Upgrade(element);
	}

	private void DispatchTileVFXRequestEvent(Tile tile, Element element)
	{
		CodeControl.Message.Send(new TileVFXRequestEvent(tile, element));
	}

	private void DispatchResourceChangeRequestEvent(int amount)
	{
		CodeControl.Message.Send(new ResourceChangeRequestEvent(amount));
	}

	private void DispatchSimulateTowerUpgradeRequestEvent(TowerController tower, Element element)
	{
		CodeControl.Message.Send(new SimulateUpgradeRequestEvent(tower, element));
	}

	private void DispatchSimulateTowerModifierRequestEvent(TowerController towerController, PropertyModifier[] propertyModifiers, float[] propertyModifierValues)
	{
		CodeControl.Message.Send(new SimulateTowerModifierRequestEvent(towerController, propertyModifiers, propertyModifierValues));
	}

	private void DispatchStopDisplayingParabolaRequestEvent()
	{
		CodeControl.Message.Send(new StopDisplayingParabolaRequestEvent());
	}

	private void DispatchTowerSoldEvent(TowerController tower)
	{
		CodeControl.Message.Send(new TowerSoldEvent(tower));
	}

	private void DispatchPropertyModifiersAppliedEvent(PropertyModifier[] propertyModifiers, float duration)
	{
		CodeControl.Message.Send(new TowerModifierAppliedEvent(duration, propertyModifiers));
	}
}
