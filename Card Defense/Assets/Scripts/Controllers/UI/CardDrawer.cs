using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDrawer : MonoBehaviour
{
	public CardHand hand;
	private Animator animator;
	private bool active = false;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		CodeControl.Message.AddListener<ToggleCardDrawerRequestEvent>(OnCardDrawerToggleRequested);
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
}
