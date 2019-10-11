using System;
using UnityEngine;

[Serializable]
public enum Element
{
	Fire,
	Water,
	Earth,
	Wind,
	Ice,
	Lightning,
	None
}

public static class ElementUtility
{
	public static ProjectileMovementType MovementForElement(Element element)
	{
		switch (element)
		{
			case Element.Fire:
				return ProjectileMovementType.StraightChain;
			case Element.Water:
				return ProjectileMovementType.BounceOnGround;
			case Element.Earth:
				return ProjectileMovementType.BounceOnGround;
			case Element.Wind:
				return ProjectileMovementType.StraightChain;
			case Element.Ice:
				return ProjectileMovementType.AOEAtTower;
			case Element.Lightning:
				return ProjectileMovementType.AOEAtTower;
		}
		return ProjectileMovementType.StraightChain;
	}

	public static int ResourceRefundForSellingElements(Element[] elements)
	{
		return elements.Length * 50;
	}

	public static float GroundHazardDurationFromElements(Element[] elements)
	{
		int waterAmount = 0;
		foreach(Element element in elements)
		{
			if (element == Element.Water) waterAmount++;
		}
		return waterAmount * 2f;
	}

	public static TowerTargeting TowerTargetingForBaseElement(Element element)
	{
		switch (element)
		{
			case Element.Fire:
				return TowerTargeting.Direct;
			case Element.Water:
				return TowerTargeting.Ground;
			case Element.Earth:
				return TowerTargeting.Ground;
			case Element.Wind:
				return TowerTargeting.Direct;
			case Element.Ice:
				return TowerTargeting.NoTarget;
			case Element.Lightning:
				return TowerTargeting.NoTarget;
		}
		return TowerTargeting.Direct;
	}

	public static Vector3 RotationAxisForChargeFromTowerElement(Element element)
	{
		if(element == Element.Ice || element == Element.Lightning)
		{
			return Vector3.up;
		}
		return Vector3.forward;
	}

	public static float DamageFromElements(Element[] elements)
	{
		float damage = 0;
		foreach(Element element in elements)
		{
			damage += ElementDamage(element);
		}
		return damage;
	}

	public static float SpeedFromElements(Element[] elements)
	{
		return 7f;
	}

	public static float AOEFromElements(Element[] elements)
	{
		return 8f;
	}

	public static int BouncesFromElements(Element[] elements)
	{
		switch (elements[0])
		{
			case Element.Earth:
				return 2;
			case Element.Water:
				return 1;
		}
		return 2;
	}

	public static int ChainLengthFromElements(Element[] elements)
	{
		if (elements[0] == Element.Fire) return 2;
		return 0;
	}

	public static float GroundHazardTimerFromElements(Element[] elements)
	{
		return 0.5f;
	}

	public static float ElementDamage(Element element)
	{
		switch (element)
		{
			case Element.Fire:
				return 2.5f;
			case Element.Water:
				return 1f;
			case Element.Lightning:
				return 0.75f;
			case Element.Earth:
				return 1f;
			case Element.Wind:
				return 0.75f;
		}
		return 0;
	}

	public static float ElementSpeed(Element element)
	{
		switch (element)
		{
			case Element.Fire:
				return 2.5f;
			case Element.Water:
				return 1f;
			case Element.Lightning:
				return 0.75f;
			case Element.Earth:
				return 1f;
			case Element.Wind:
				return 0.75f;
		}
		return 0;
	}
}