using System;
using UnityEngine;

public enum LocalizeDefs
{
    Health = 0x0,
    Damage,
    PressStart,
	None,
}

public class Localization : MonoBehaviour
{
    public LocalizeCollection localizeCollection;
	//public ElementSpriteCollection elementSpriteCollection;
	private void Awake()
	{
		
	}
}
