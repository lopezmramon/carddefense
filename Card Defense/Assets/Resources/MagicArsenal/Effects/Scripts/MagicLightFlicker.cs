using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicLightFlicker : MonoBehaviour
{
	// Properties
	public string waveFunction = "sin"; 
	public float baseFloat = 0.0f; 
	public float amplitude = 1.0f; 
	public float phase = 0.0f; 
	public float frequency = 0.5f; 

	private Color originalColor;
	private new Light light;
	private bool turnedOn;

	private void Start()
	{
		light = GetComponent<Light>();
		originalColor = GetComponent<Light>().color;
	}

	private void Update()
	{
		if (turnedOn)
		{
			FlickerDependingOnWave();
		}
	}

	private void FlickerDependingOnWave()
	{
		light.color = originalColor * (EvalWave());
	}

	internal void Toggle(bool v)
	{
		turnedOn = false;
	}

	private float EvalWave()
	{
		float x = (Time.time + phase) * frequency;
		float y;

		x = x - Mathf.Floor(x); // normalized value (0..1)

		if (waveFunction == "sin")
		{
			y = Mathf.Sin(x * 2 * Mathf.PI);
		}
		else if (waveFunction == "tri")
		{
			if (x < 0.5f)
				y = 4.0f * x - 1.0f;
			else
				y = -4.0f * x + 3.0f;
		}
		else if (waveFunction == "sqr")
		{
			if (x < 0.5)
				y = 1.0f;
			else
				y = -1.0f;
		}
		else if (waveFunction == "saw")
		{
			y = x;
		}
		else if (waveFunction == "inv")
		{
			y = 1.0f - x;
		}
		else if (waveFunction == "noise")
		{
			y = 1 - (UnityEngine.Random.value * 2);
		}
		else
		{
			y = 1.0f;
		}
		return (y * amplitude) + baseFloat;
	}

}
