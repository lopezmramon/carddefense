[System.Serializable]
public enum Element
{
	Fire,
	Water,
	Earth,
	Wind,
	Ice,
	Lightning
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
				return ProjectileMovementType.StraightChain;
			case Element.Earth:
				return ProjectileMovementType.BounceOnGround;
			case Element.Wind:
				return ProjectileMovementType.StraightChain;
			case Element.Ice:
				return ProjectileMovementType.StraightChain;
			case Element.Lightning:
				return ProjectileMovementType.AOEAtTower;
		}
		return ProjectileMovementType.StraightChain;
	}
}