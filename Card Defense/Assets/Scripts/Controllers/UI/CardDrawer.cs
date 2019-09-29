using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDrawer : MonoBehaviour
{
	public List<CardContainer> hand = new List<CardContainer>();
	private Animator animator;
	private bool active = false;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		CodeControl.Message.AddListener<ToggleCardDrawerRequestEvent>(OnCardDrawerToggleRequested);
		CodeControl.Message.AddListener<CardDrawnEvent>(OnCardDrawn);
		CodeControl.Message.AddListener<CardConsumedEvent>(OnCardConsumed);
		CodeControl.Message.AddListener<EmptyHandRequestEvent>(OnEmptyHandRequested);
		CodeControl.Message.AddListener<RedrawHandRequestEvent>(OnRedrawHandRequested);
		CodeControl.Message.AddListener<CardPickStartRequestEvent>(OnCardPickStartRequested);
		CodeControl.Message.AddListener<CardPickEndedEvent>(OnCardPickEnded);
	}

	private void OnCardPickStartRequested(CardPickStartRequestEvent obj)
	{
		foreach (CardContainer cardContainer in hand)
		{
			cardContainer.ToggleCardPicking(true);
		}
	}

	private void OnCardPickEnded(CardPickEndedEvent obj)
	{
		foreach (CardContainer cardContainer in hand)
		{
			cardContainer.ToggleCardPicking(false);
		}
	}

	private void OnCardConsumed(CardConsumedEvent obj)
	{
		hand.Remove(obj.consumedCard);
	}

	private void OnEmptyHandRequested(EmptyHandRequestEvent obj)
	{
		EmptyHand();
	}

	private void OnCardDrawn(CardDrawnEvent obj)
	{
		hand.Add(obj.card);
	}

	private void OnRedrawHandRequested(RedrawHandRequestEvent obj)
	{
		RedrawHand();
	}

	private void RedrawHand()
	{
		int currentHandSize = transform.childCount;
		EmptyHand();
		DispatchDrawRandomCardsRequestEvent(currentHandSize);
	}

	private void EmptyHand()
	{
		foreach (CardContainer cardContainer in hand)
		{
			Destroy(cardContainer.gameObject);
		}
		hand.Clear();
	}

	public void TestActivate()
	{
		CodeControl.Message.Send(new ToggleCardDrawerRequestEvent());
	}

	private void OnCardDrawerToggleRequested(ToggleCardDrawerRequestEvent obj)
	{
		active = !active;
		animator.SetTrigger(active ? "Activate" : "Deactivate");
	}

	private void DispatchDrawRandomCardsRequestEvent(int amount)
	{
		CodeControl.Message.Send(new DrawRandomCardsRequestEvent(amount));
	}
}
