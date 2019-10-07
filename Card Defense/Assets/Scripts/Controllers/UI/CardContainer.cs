using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardContainer : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	public Image background, illustration, resource, raycastTarget;
	public Text description, cost, cardName;
	private Transform originalParent;
	public Card card;
	private bool followMouse = false;
	private Vector3 offset;
	public bool pickingCards;
	public void ToggleCardPicking(bool pickingCards)
	{
		this.pickingCards = pickingCards;
	}

	public void Initialize(Card card)
	{
		this.card = card;
		raycastTarget = GetComponent<Image>();
		description.text = card.description;
		cost.text = card.cost.ToString();
		cardName.text = card.name;
		background.sprite = card.backgroundSprite;
		illustration.sprite = card.illustrationSprite;
		resource.sprite = card.resourceSprite;
	}

	private void Awake()
	{
		originalParent = transform.parent;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		AnalyzePointerDown(eventData);
	}

	private void AnalyzePointerDown(PointerEventData eventData)
	{
		if (pickingCards)
		{
			DispatchCardPickedEvent();
		}
		else
		{
			StartFollowingMouseCursor();
			DispatchCardGrabbedEvent();
			background.raycastTarget = false;
			raycastTarget.raycastTarget = false;
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		AnalyzePointerUp(eventData);
	}

	private void StartFollowingMouseCursor()
	{
		transform.localScale = Vector3.one / 2;
		offset = transform.position - Input.mousePosition;
		transform.SetParent(transform.root);
		followMouse = true;
	}

	private void AnalyzePointerUp(PointerEventData eventData)
	{
		if (pickingCards)
		{

		}
		else
		{
			bool overDrawer = eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.name.Contains("Card");
			DispatchCardDroppedEvent(overDrawer);
			background.raycastTarget = true;
			raycastTarget.raycastTarget = true;
			followMouse = false;
			transform.SetParent(originalParent);
			transform.position = Vector3.zero;
			transform.localScale = Vector3.one;
		}
	}

	private void Update()
	{
		if (followMouse)
		{
			transform.position = Input.mousePosition + offset / 2;
		}
	}

	public void Discard()
	{
		Destroy(gameObject);
	}

	private void DispatchCardGrabbedEvent()
	{
		CodeControl.Message.Send(new CardGrabbedEvent(this));
	}

	private void DispatchCardDroppedEvent(bool overDrawer)
	{
		CodeControl.Message.Send(new CardDroppedEvent(this, overDrawer));
	}

	private void DispatchCardPickedEvent()
	{
		CodeControl.Message.Send(new CardPickRequestEvent(this));
	}
}
