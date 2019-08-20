using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PhysicsRaycastController : MonoBehaviour
{
	private PhysicsRaycaster physicsRaycaster;
	private List<RaycastResult> raycastResults = new List<RaycastResult>();
	private PointerEventData pointerEventData;
	private CardContainer grabbedCard;

	private void Awake()
	{
		physicsRaycaster = GetComponent<PhysicsRaycaster>();
		CodeControl.Message.AddListener<CardGrabbedEvent>(OnCardGrabbed);
		CodeControl.Message.AddListener<CardDroppedEvent>(OnCardDropped);
		pointerEventData = new PointerEventData(EventSystem.current);
	}

	private void OnCardGrabbed(CardGrabbedEvent obj)
	{
		grabbedCard = obj.card;
	}

	private void OnCardDropped(CardDroppedEvent obj)
	{
		grabbedCard = null;
	}

	private void Update()
	{
		if (grabbedCard != null)
		{
			CastRay();
		}
	}

	private void CastRay()
	{
		if (Input.GetAxis("Mouse X") == 0 && Input.GetAxis("Mouse Y") == 0)
		{
			return;
		}

		pointerEventData.position = Input.mousePosition;
		raycastResults.Clear();
		physicsRaycaster.Raycast(pointerEventData, raycastResults);
		if (raycastResults.Count == 0)
		{

		}
		else
		{
			foreach (RaycastResult raycastResult in raycastResults)
			{
				if (raycastResult.gameObject.GetComponent<TileController>())
				{
					DispatchCardOverBuildableTileEvent(grabbedCard.card, raycastResult.gameObject.GetComponent<TileController>().tile);
				}
			}
		}
	}

	private void DispatchCardOverBuildableTileEvent(Card card, Tile tile)
	{
		//CodeControl.Message.Send(new CardOverTileEvent(card, tile));
	}

}
