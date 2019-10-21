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
	public static ProjectileMovementType Movement(Element element)
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

	public static float GroundHazardDuration(Element[] elements)
	{
		int waterAmount = 0;
		foreach (Element element in elements)
		{
			if (element == Element.Water) waterAmount++;
		}
		return waterAmount * 2f;
	}

	public static float ProjectileDamage(Element[] elements)
	{
		float damage = 0;
		foreach (Element element in elements)
		{
			damage += ElementDamage(element);
		}
		return damage;
	}

	public static float ElementDamage(Element element)
	{
		switch (element)
		{
			case Element.Fire:
				return 2f;
			case Element.Water:
				return 1f;
			case Element.Lightning:
				return 0.75f;
			case Element.Earth:
				return 0.5f;
			case Element.Wind:
				return 0.75f;
			case Element.Ice:
				return 0.75f;
		}
		return 0;
	}

	public static float ProjectileSpeed(Element[] elements)
	{
		float speed = 3f;
		foreach (Element element in elements)
		{
			speed += ElementProjectileSpeedModifier(element);
		}
		return speed;
	}

	public static float Slow(Element[] elements)
	{
		float slow = 0;
		foreach (Element element in elements)
		{
			if (slow == 0)
				slow += ElementSlow(element);
			else
				slow *= (1 + ElementSlow(element));
		}
		return slow;
	}

	public static float AoE(Element[] elements)
	{
		float aoe = BaseElementAOE(elements[0]);
		for (int i = 1; i < elements.Length; i++)
		{
			aoe += ElementAoE(elements[i]);
		}
		return aoe;
	}

	public static float BaseElementAOE(Element element)
	{
		switch (element)
		{
			case Element.Fire:
			case Element.Water:
			case Element.Wind:
			case Element.None:
				return 0;
			case Element.Ice:
				return 5f;
			case Element.Lightning:
				return 5.5f;
			case Element.Earth:
				return 3f;
		}
		return 0;
	}

	public static float ElementAoE(Element element)
	{
		switch (element)
		{
			case Element.Fire:
			case Element.Water:
			case Element.Wind:
			case Element.Ice:
			case Element.Lightning:
			case Element.None:
				return 0;
			case Element.Earth:
				return 0.15f;
		}
		return 0;
	}

	public static int Bounces(Element[] elements)
	{
		int bounces = 0;
		float partialBounces = 0;
		bounces += (int)BaseBounceForElement(elements[0]);
		for (int i = 1; i < elements.Length; i++)
		{
			switch (elements[i])
			{
				case Element.Earth:
					bounces += 1;
					break;
				case Element.Water:
					partialBounces += 0.5f;
					if (partialBounces >= 1)
					{
						partialBounces = 0;
						bounces += 1;
					}
					break;
			}
		}
		return bounces;
	}

	public static float BaseBounceForElement(Element element)
	{
		switch (element)
		{
			case Element.Fire:
			case Element.Wind:
			case Element.Ice:
			case Element.Lightning:
			case Element.None:
				break;
			case Element.Water:
				return 1;
			case Element.Earth:
				return 2;
		}
		return 0;
	}

	public static float ElementBounces(Element element)
	{
		float bounces = 0;
		switch (element)
		{
			case Element.Fire:
				break;
			case Element.Water:
				bounces += 0.5f;
				break;
			case Element.Earth:
				bounces++;
				break;
			case Element.Wind:
				break;
			case Element.Ice:
				break;
			case Element.Lightning:
				break;
			case Element.None:
				break;
		}
		return bounces;
	}

	internal static float ElementSlow(Element element)
	{
		float slow = 0;
		switch (element)
		{
			case Element.Fire:
			case Element.Water:
			case Element.Earth:
			case Element.Wind:
			case Element.Lightning:
			case Element.None:
				break;
			case Element.Ice:
				slow += 0.25f;
				break;
		}
		return slow;
	}

	public static int ChainLength(Element[] elements)
	{
		int length = 0;
		float partialChain = 0;
		for (int i = 0; i < elements.Length; i++)
		{
			switch (elements[i])
			{
				case Element.Lightning:
					length += 1;
					break;
				case Element.Wind:
					partialChain += 0.5f;
					if (partialChain >= 1)
					{
						partialChain = 0;
						length += 1;
					}
					break;
			}
		}
		return length;
	}

	public static float ElementChain(Element element)
	{
		float chain = 0;
		switch (element)
		{
			case Element.Fire:
				break;
			case Element.Water:
				break;
			case Element.Earth:
				break;
			case Element.Wind:
				chain += 0.5f;
				break;
			case Element.Ice:
				break;
			case Element.Lightning:
				chain++;
				break;
			case Element.None:
				break;
		}
		return chain;
	}

	public static float BaseTowerRange(Element element)
	{
		switch (element)
		{
			case Element.Fire:
				return 5f;
			case Element.Water:
				return 7f;
			case Element.Ice:
			case Element.Lightning:
				return 5f;
			case Element.Earth:
				return 7f;
			case Element.Wind:
				return 5f;
		}
		return 0;
	}

	public static float ElementRange(Element element)
	{
		switch (element)
		{
			case Element.Fire:
				return 0.1f;
			case Element.Water:
				return 0.1f;
			case Element.Lightning:
				return -0.1f;
			case Element.Earth:
				return 0.25f;
			case Element.Wind:
				return 0.15f;
		}
		return 0;
	}

	public static float TowerRange(Element[] elements)
	{
		float range = BaseTowerRange(elements[0]);
		for (int i = 1; i < elements.Length; i++)
		{
			range += ElementRange(elements[i]);
		}
		return range;
	}

	public static float BaseElementFireRate(Element element)
	{
		switch (element)
		{
			case Element.Fire:
				return 1.25f;
			case Element.Water:
				return 0.85f;
			case Element.Lightning:
				return 0.75f;
			case Element.Earth:
				return 0.6f;
			case Element.Wind:
				return 1.55f;
			case Element.Ice:
				return 1f;
		}
		return 0;
	}

	public static float ElementFireRate(Element element)
	{
		switch (element)
		{
			case Element.Fire:
				return 0.25f;
			case Element.Water:
				return -0.15f;
			case Element.Lightning:
				return 0.15f;
			case Element.Earth:
				return -0.125f;
			case Element.Wind:
				return 0.35f;
		}
		return 0;
	}

	public static float FireRate(Element[] elements)
	{
		float fireRate = BaseElementFireRate(elements[0]);
		for (int i = 1; i < elements.Length; i++)
		{
			fireRate += ElementFireRate(elements[i]);
		}
		return fireRate;
	}

	public static float ElementProjectileSpeedModifier(Element element)
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
		if (element == Element.Ice || element == Element.Lightning)
		{
			return Vector3.up;
		}
		return Vector3.forward;
	}

	public static int SaleValue(Element[] elements)
	{
		return elements.Length * 50;
	}
}