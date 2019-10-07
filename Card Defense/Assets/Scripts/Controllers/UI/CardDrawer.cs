using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
		CodeControl.Message.AddListener<RedrawRandomCardsRequestEvent>(OnRedrawRandomCardsRequestedd);
		CodeControl.Message.AddListener<CardPickStartedEvent>(OnCardPickStarted);
		CodeControl.Message.AddListener<CardPickEndedEvent>(OnCardPickEnded);
	}

	private void OnRedrawRandomCardsRequestedd(RedrawRandomCardsRequestEvent obj)
	{
		RedrawRandomCards(obj.amount);
	}

	private void OnCardPickStarted(CardPickStartedEvent obj)
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
		if(hand.Contains(obj.consumedCard))
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

	private void RedrawRandomCards(int amount)
	{
		if (amount > transform.childCount) amount = transform.childCount;
		CardContainer[] cardsToRedraw = new CardContainer[amount];
		cardsToRedraw = hand.OrderBy(x => Guid.NewGuid()).Take(amount).ToArray();
		for(int i = 0; i < cardsToRedraw.Length; i++)
		{
			cardsToRedraw[i].Discard();
			hand.Remove(cardsToRedraw[i]);
		}
		DispatchDrawRandomCardsRequestEvent(amount);
	}

	private void EmptyHand()
	{
		foreach (CardContainer cardContainer in hand)
		{
			cardContainer.Discard();
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
