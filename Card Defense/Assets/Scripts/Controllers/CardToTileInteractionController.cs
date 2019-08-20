using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardToTileInteractionController : MonoBehaviour
{
	private CardContainer grabbedCard;
	private Tile hoveredTile;

	private void Awake()
	{
		CodeControl.Message.AddListener<CardGrabbedEvent>(OnCardGrabbed);
		CodeControl.Message.AddListener<CardDroppedEvent>(OnCardDropped);
		CodeControl.Message.AddListener<CardConsumedEvent>(OnCardConsumed);
		CodeControl.Message.AddListener<CursorEnterTileEvent>(OnCursorEnterTile);
		CodeControl.Message.AddListener<CursorExitTileEvent>(OnCursorExitTile);
	}

	private void OnCursorEnterTile(CursorEnterTileEvent obj)
	{
		hoveredTile = obj.tile;
		if (grabbedCard == null)
		{

		}
		else
		{
			DispatchCardOverTileEvent(grabbedCard, hoveredTile);
		}
	}

	private void OnCursorExitTile(CursorExitTileEvent obj)
	{
		hoveredTile = null;
		DispatchCardNoLongerOverTileEvent();
	}

	private void OnCardConsumed(CardConsumedEvent obj)
	{
		grabbedCard = null;
	}

	private void OnCardDropped(CardDroppedEvent obj)
	{
		grabbedCard = null;
	}

	private void OnCardGrabbed(CardGrabbedEvent obj)
	{
		grabbedCard = obj.card;
	}

	private void DispatchCardOverTileEvent(CardContainer card, Tile tile)
	{
		CodeControl.Message.Send(new CardOverTileEvent(card, tile));
	}

	private void DispatchCardNoLongerOverTileEvent()
	{
		CodeControl.Message.Send(new CardNoLongerOverTileEvent());
	}
}
