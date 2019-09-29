using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitGroup
{
	public List<OrbitMovement> orbits;

	public OrbitGroup()
	{
		orbits = new List<OrbitMovement>();
	}

	public OrbitMovement AddOrbitMovement(Transform transform, Transform center,
		float radius, float radiusSpeed, float rotationSpeed, Element towerElement)
	{
		OrbitMovement newOrbit = new OrbitMovement(transform, center, RotationAxisDependingOnNumberOfOrbits(towerElement), radius, radiusSpeed, rotationSpeed);
		orbits.Add(newOrbit);
		return newOrbit;
	}

	public void RotateAllOrbits()
	{
		foreach (OrbitMovement orbit in orbits)
		{
			orbit.Rotate();
		}
	}

	public Vector3 RotationAxisDependingOnNumberOfOrbits(Element element)
	{
		Vector3 axis = Vector3.forward;
		if (orbits.Count == 0) return axis;
		axis = orbits[orbits.Count - 1].axis;
		switch (orbits.Count)
		{
			case 1:
				axis.x = 0;
				axis.z = 0;
				axis.y = 1;
				return axis;
			case 2:
				axis.y = 0;
				axis.z = 0;
				axis.x = 1;
				return axis;
			case 3:
				axis.y = 0.5f;
				axis.z = 0.5f;
				axis.x = 0.5f;
				return axis;
			case 4:
				axis.y = 0f;
				axis.z = 0.5f;
				axis.x = 0f;
				return axis;
			case 5:
				axis.y = 0.5f;
				axis.z = 0;
				axis.x = 0;
				return axis;
			case 6:
				axis.y = 0;
				axis.z = 0;
				axis.x = 0.5f;
				return axis;
			case 7:
				axis.y = 0.75f;
				axis.z = 0.75f;
				axis.x = 0.75f;
				return axis;
			case 8:
				axis.y = 0.75f;
				axis.z = 0f;
				axis.x = 0.75f;
				return axis;
			case 9:
				axis.y = 0.5f;
				axis.z = 0f;
				axis.x = 0.5f;
				return axis;
			case 10:
				axis.y = 0f;
				axis.z = 0.5f;
				axis.x = 0.5f;
				return axis;
		}
		return axis;
	}

	internal void AddOrbitMovement(Transform transform, Transform chargeRotationCenter, object radius, object radiusSpeed, object rotationSpeed, Element element)
	{
		throw new NotImplementedException();
	}
}
