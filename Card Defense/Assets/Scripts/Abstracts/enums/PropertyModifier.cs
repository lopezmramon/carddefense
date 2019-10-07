using System;
using UnityEngine;
[Serializable]
public enum PropertyModifier
{
	None,
	FireRate,
	Damage,
	AOE,
	Range,
}

public static class PropertyModifierHelper
{
	public static MeshRenderer FindCorrectMeshRendererForProperty(TowerController tower, PropertyModifier[] propertyModifiers)
	{
		MeshRenderer meshRenderer = null;
		switch (propertyModifiers[0])
		{
			case PropertyModifier.AOE:
			case PropertyModifier.Range:
				meshRenderer = tower.TowerBaseRenderer;
				break;
			case PropertyModifier.Damage:
				meshRenderer = tower.TowerTurretRenderer;
				break;
			case PropertyModifier.FireRate:
				meshRenderer = tower.TowerBarrelRenderer;
				break;
		}
		if (meshRenderer == null)
		{
			meshRenderer = tower.TowerBaseRenderer;
		}
		return meshRenderer;

	}
}


