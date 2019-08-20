using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
	public static int currentResourceAmount;
	public static int infiniteResourcePowerupAmount;

	private void Awake()
	{
		CodeControl.Message.AddListener<ResourceChangeRequestEvent>(OnResourceChangeRequested);
		CodeControl.Message.AddListener<InfiniteResourcePowerupActivatedEvent>(OnInfiniteResourcePowerupActivated);
		currentResourceAmount = 200;
	}

	private void OnInfiniteResourcePowerupActivated(InfiniteResourcePowerupActivatedEvent obj)
	{
		infiniteResourcePowerupAmount++; 
	}

	private void OnResourceChangeRequested(ResourceChangeRequestEvent obj)
	{
		currentResourceAmount += obj.amount;
		if(obj.amount < 0 && infiniteResourcePowerupAmount > 0)
		{
			infiniteResourcePowerupAmount--;
		}
	}

	private void DispatchResourceChangedEvent()
	{
		CodeControl.Message.Send(new ResourceChangedEvent(currentResourceAmount));
	}
}
