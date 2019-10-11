using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargerCardUIDisplayRequestEvent : MonoBehaviour
{
	public Card card;

	public LargerCardUIDisplayRequestEvent(Card card)
	{
		this.card = card;
	}
}
