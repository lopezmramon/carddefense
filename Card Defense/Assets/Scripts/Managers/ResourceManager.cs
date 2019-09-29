using System;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
	public static int currentResourceAmount;
	public static int infiniteResourcePowerupAmount;

	private void Awake()
	{
		CodeControl.Message.AddListener<ResourceChangeRequestEvent>(OnResourceChangeRequested);
		CodeControl.Message.AddListener<InfiniteResourcePowerupActivatedEvent>(OnInfiniteResourcePowerupActivated);
		CodeControl.Message.AddListener<TowerSoldEvent>(OnTowerSold);
		CodeControl.Message.AddListener<LevelReadyEvent>(OnLevelReady);
	}

	private void OnLevelReady(LevelReadyEvent obj)
	{
		ChangeResource(obj.level.startingTowerBuildingResource);
	}

	private void OnTowerSold(TowerSoldEvent obj)
	{
		ChangeResource(ElementUtility.ResourceRefundForSellingElements(obj.tower.elements.ToArray()));
	}

	private void OnInfiniteResourcePowerupActivated(InfiniteResourcePowerupActivatedEvent obj)
	{
		infiniteResourcePowerupAmount++; 
	}

	private void OnResourceChangeRequested(ResourceChangeRequestEvent obj)
	{
		ChangeResource(obj.amount);
	}

	private void ChangeResource(int change)
	{
		currentResourceAmount += change;
		if (change < 0 && infiniteResourcePowerupAmount > 0)
		{
			infiniteResourcePowerupAmount--;
		}
		DispatchResourceChangedEvent();
	}

	private void DispatchResourceChangedEvent()
	{
		CodeControl.Message.Send(new ResourceChangedEvent(currentResourceAmount));
	}
}
