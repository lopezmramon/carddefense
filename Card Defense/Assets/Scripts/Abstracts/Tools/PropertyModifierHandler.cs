using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PropertyModifierHandler
{
	public float fireRateMultiplier = 1f, rangeMultiplier = 1f, projectileDamageMultiplier = 1f, projectileAOEMultiplier = 1f, projectileSpeedMultiplier;
	public float fireRateMultiplierTimer, rangeMultiplierTimer, projectileDamageMultiplierTimer, projectileAOEMultiplierTimer, projectileSpeedMultiplierTimer;
	private Dictionary<PropertyModifier, List<GameObject>> particles = new Dictionary<PropertyModifier, List<GameObject>>();
	private System.Action<GameObject> OnParticleEnded;
	public System.Action<PropertyModifier> OnPropertyReset, OnPropertyApplied;
	public List<PropertyModifier> activeModifiers = new List<PropertyModifier>();

	public PropertyModifierHandler(System.Action<GameObject> OnParticleEnded)
	{
		particles.Add(PropertyModifier.AOE, new List<GameObject>());
		particles.Add(PropertyModifier.Damage, new List<GameObject>());
		particles.Add(PropertyModifier.FireRate, new List<GameObject>());
		particles.Add(PropertyModifier.Range, new List<GameObject>());
		this.OnParticleEnded = OnParticleEnded;
	}

	public void AddParticle(PropertyModifier propertyModifier, GameObject particle)
	{
		particles[propertyModifier].Add(particle);
	}

	public void AddParticles(PropertyModifier propertyModifier, List<GameObject> particles)
	{
		this.particles[propertyModifier].AddRange(particles);
	}

	public void PropertyCountdowns()
	{
		if (fireRateMultiplierTimer >= 0)
		{
			fireRateMultiplierTimer -= Time.deltaTime * GameManager.gameSpeedMultiplier;
		}
		else if (activeModifiers.Contains(PropertyModifier.FireRate))
		{
			PropertyReset(PropertyModifier.FireRate);
		}
		if (projectileDamageMultiplierTimer >= 0)
		{
			projectileDamageMultiplierTimer -= Time.deltaTime * GameManager.gameSpeedMultiplier;
		}
		else if (activeModifiers.Contains(PropertyModifier.Damage))
		{
			PropertyReset(PropertyModifier.Damage);
		}
		if (projectileAOEMultiplierTimer >= 0)
		{
			projectileAOEMultiplierTimer -= Time.deltaTime * GameManager.gameSpeedMultiplier;
		}
		else if (activeModifiers.Contains(PropertyModifier.AOE))
		{
			PropertyReset(PropertyModifier.AOE);
		}
		if (rangeMultiplierTimer >= 0)
		{
			rangeMultiplierTimer -= Time.deltaTime * GameManager.gameSpeedMultiplier;
		}
		else if (activeModifiers.Contains(PropertyModifier.Range))
		{
			PropertyReset(PropertyModifier.Range);
		}
		if (projectileSpeedMultiplierTimer >= 0)
		{
			projectileSpeedMultiplierTimer -= Time.deltaTime * GameManager.gameSpeedMultiplier;
		}
		else if (activeModifiers.Contains(PropertyModifier.ProjectileSpeed))
		{
			PropertyReset(PropertyModifier.ProjectileSpeed);
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
			case PropertyModifier.ProjectileSpeed:
				projectileSpeedMultiplier = 1;
				break;
		}
		OnPropertyReset?.Invoke(propertyModifier);
		RemoveParticlesForModifier(propertyModifier);
	}

	public void AddProjectileDamageMultiplier(float increase, float duration)
	{
		projectileDamageMultiplier += increase;
		projectileDamageMultiplierTimer += duration;
		activeModifiers.Add(PropertyModifier.Damage);
		OnPropertyApplied?.Invoke(PropertyModifier.Damage);
	}

	public void AddProjectileSpeedMultiplier(float increase, float duration)
	{
		rangeMultiplier += increase;
		rangeMultiplierTimer += duration;
		activeModifiers.Add(PropertyModifier.ProjectileSpeed);
		OnPropertyApplied?.Invoke(PropertyModifier.ProjectileSpeed);
	}

	public void AddFireRateMultiplier(float increase, float duration)
	{
		fireRateMultiplier += increase;
		fireRateMultiplierTimer += duration;
		activeModifiers.Add(PropertyModifier.FireRate);
		OnPropertyApplied?.Invoke(PropertyModifier.FireRate);
	}

	public void AddAOEMultiplier(float increase, float duration)
	{
		projectileAOEMultiplier += increase;
		projectileAOEMultiplierTimer += duration;
		activeModifiers.Add(PropertyModifier.Damage);

		OnPropertyApplied?.Invoke(PropertyModifier.AOE);
	}

	private void RemoveParticlesForModifier(PropertyModifier propertyModifier)
	{
		activeModifiers.Remove(propertyModifier);
		foreach (GameObject particle in particles[propertyModifier])
		{
			OnParticleEnded(particle);
		}
	}
}
