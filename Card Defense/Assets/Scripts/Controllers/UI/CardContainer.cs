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
		StartFollowingMouseCursor();
		DispatchCardGrabbedEvent();
		background.raycastTarget = false;
		raycastTarget.raycastTarget = false;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		AnalyzeMouseUp(eventData);
		DispatchCardDroppedEvent();
		background.raycastTarget = true;
		raycastTarget.raycastTarget = true;
	}

	private void StartFollowingMouseCursor()
	{
		transform.localScale = Vector3.one / 2;
		transform.SetParent(transform.root);
		followMouse = true;
	}

	private void AnalyzeMouseUp(PointerEventData eventData)
	{
		followMouse = false;
		transform.SetParent(originalParent);
		transform.position = Vector3.zero;
		transform.localScale = Vector3.one;
	}

	private void Update()
	{
		if (followMouse)
		{
			transform.position = Input.mousePosition;
		}
	}

	private void DispatchCardGrabbedEvent()
	{
		CodeControl.Message.Send(new CardGrabbedEvent(this));
	}

	private void DispatchCardDroppedEvent()
	{
		CodeControl.Message.Send(new CardDroppedEvent(this));
	}

}
