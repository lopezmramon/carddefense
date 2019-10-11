using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PropertyModifierHandler
{
	public float fireRateMultiplier = 1f, rangeMultiplier = 1f, projectileDamageMultiplier = 1f, projectileAOEMultiplier = 1f;
	public float fireRateMultiplierTimer, rangeMultiplierTimer, projectileDamageMultiplierTimer, projectileAOEMultiplierTimer;
	private Dictionary<PropertyModifier, List<GameObject>> particles = new Dictionary<PropertyModifier, List<GameObject>>();
	private System.Action<GameObject> OnParticleEnded;

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

	public void AddParticles(PropertyModifier propertyModifier,List<GameObject> particles)
	{
		this.particles[propertyModifier].AddRange(particles);
	}

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
		RemoveParticlesForModifier(propertyModifier);
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

	private void RemoveParticlesForModifier(PropertyModifier propertyModifier)
	{
		foreach (GameObject particle in particles[propertyModifier])
		{
			OnParticleEnded(particle);
		}
	}
}
