using System;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
	public static int currentResourceAmount;
	public static int infiniteResourcePowerupAmount;
	public float multiplierCountdown;
	private int multiplier;

	private void Awake()
	{
		CodeControl.Message.AddListener<ResourceChangeRequestEvent>(OnResourceChangeRequested);
		CodeControl.Message.AddListener<InfiniteResourcePowerupActivatedEvent>(OnInfiniteResourcePowerupActivated);
		CodeControl.Message.AddListener<ResourceMultiplierStartRequestEvent>(OnResourceMultiplierStartRequested);
		CodeControl.Message.AddListener<TowerSoldEvent>(OnTowerSold);
		CodeControl.Message.AddListener<LevelReadyEvent>(OnLevelReady);
	}

	private void OnResourceMultiplierStartRequested(ResourceMultiplierStartRequestEvent obj)
	{
		multiplier = (int)obj.multiplier;
		multiplierCountdown = obj.duration;
		DispatchResourceMultiplierStartedEvent();
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

	private void Update()
	{
		if (multiplierCountdown > 0)
		{
			multiplierCountdown -= Time.deltaTime * GameManager.gameSpeedMultiplier;
		}
		else
		{
			multiplier = 1;
			multiplierCountdown = 0;
			DispatchResourceMultiplierEndedEvent();
		}
	}

	private void ChangeResource(int change)
	{
		if (change > 0)
		{
			currentResourceAmount += change * multiplier;
		}
		else
		{
			currentResourceAmount += change;
		}
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

	private void DispatchResourceMultiplierStartedEvent()
	{
		CodeControl.Message.Send(new ResourceMultiplierStartedEvent(multiplier));
	}

	private void DispatchResourceMultiplierEndedEvent()
	{
		CodeControl.Message.Send(new ResourceMultiplierEndedEvent());
	}
}
