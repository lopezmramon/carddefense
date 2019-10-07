using UnityEngine;
public class PropertyModifierHandler 
{
	public float fireRateMultiplier = 1f, rangeMultiplier = 1f, projectileDamageMultiplier = 1f, projectileAOEMultiplier = 1f;
	public float fireRateMultiplierTimer, rangeMultiplierTimer, projectileDamageMultiplierTimer, projectileAOEMultiplierTimer;

	public void PropertyCountdowns()
	{		
		if (fireRateMultiplierTimer >= 0)
		{
			fireRateMultiplierTimer -= Time.deltaTime * GameManager.gameSpeedMultiplier;
		}
		else
		{
			PropertyReset(PropertyModifier.FireRate);
		}
		if (projectileDamageMultiplierTimer >= 0)
		{
			projectileDamageMultiplierTimer -= Time.deltaTime * GameManager.gameSpeedMultiplier;
		}
		else
		{
			PropertyReset(PropertyModifier.Damage);
		}
		if (projectileAOEMultiplierTimer >= 0)
		{
			projectileAOEMultiplierTimer -= Time.deltaTime * GameManager.gameSpeedMultiplier;
		}
		else
		{
			PropertyReset(PropertyModifier.AOE);
		}
		if (rangeMultiplierTimer >= 0)
		{
			rangeMultiplierTimer -= Time.deltaTime * GameManager.gameSpeedMultiplier;
		}
		else
		{
			PropertyReset(PropertyModifier.Range);
		}
	}

	private void PropertyReset(PropertyModifier propertyModifier)
	{
		switch (propertyModifier)
		{
			case PropertyModifier.Damage:
				projectileDamageMultiplier = 1;
				break;
			case PropertyModifier.FireRate:
				fireRateMultiplier = 1;
				break;
			case PropertyModifier.AOE:
				projectileAOEMultiplier = 1;
				break;
			case PropertyModifier.Range:
				rangeMultiplier = 1;
				break;
		}
	}

	public void AddProjectileDamageMultiplier(float increase, float duration)
	{
		projectileDamageMultiplier += increase;
		projectileDamageMultiplierTimer += duration;
	}

	public void AddProjectileSpeedMultiplier(float increase, float duration)
	{
		rangeMultiplier += increase;
		rangeMultiplierTimer += duration;
	}

	public void AddFireRateMultiplier(float increase, float duration)
	{
		fireRateMultiplier += increase;
		fireRateMultiplierTimer += duration;
	}

	public void AddAOEMultiplier(float increase, float duration)
	{
		projectileAOEMultiplier += increase;
		projectileAOEMultiplierTimer += duration;
	}
}
