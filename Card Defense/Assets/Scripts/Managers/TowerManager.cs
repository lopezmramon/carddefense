using System;
using System.Collections;
using System.Collections.Generic;
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
		GenerateVisualTowers();
	}

	private void OnCardDropped(CardDroppedEvent obj)
	{
		if (hoverTile == null || cardOverTile == null || cardOverTile.card.cardType != CardType.Tower) return;
		if(ResourceManager.currentResourceAmount >= cardOverTile.card.cost)
		{
			ProcessRequest(hoverTile, cardOverTile.card.element);
			DispatchResourceChangeRequestEvent(-cardOverTile.card.cost);
			DispatchCardConsumedEvent();
		}
		else
		{
			DeactivateAllVisualTowers();			
		}
	}

	private void DispatchCardConsumedEvent()
	{
		CodeControl.Message.Send(new CardConsumedEvent(cardOverTile));
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
		if (towersPlaced.ContainsKey(obj.tile))
		{
			if(obj.card.card.cardType == CardType.Tower)
			{
				towersPlaced[obj.tile].SimulateUpgrade(obj.card.card.element);
			}
		}
		else
		{
			DispatchTileVFXRequestEvent(obj.tile, obj.card.card.element);
			PlaceVisualTowerAtTile(obj.tile, obj.card.card.element);
		}
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
		tower.transform.localPosition = new Vector3(0, 0.5f, 0);
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
}
