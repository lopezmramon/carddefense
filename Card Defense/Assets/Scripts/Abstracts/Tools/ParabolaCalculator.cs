using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ParabolaCalculator
{
	public static Parabola[] CalculateFullTrajectory(Vector3 startingPos, Vector3 target, float speed, float arcHeight, float xReductionPerBounce, int bounces)
	{
		Parabola[] parabolas = new Parabola[bounces];
		float xDistance = target.x- startingPos.x;
		for (int i = 0; i < bounces; i++)
		{
			parabolas[i] = CalculateParabola(startingPos, target, speed, arcHeight);
			startingPos = target;
		//	xDistance /= xReductionPerBounce;
			target.x += xDistance;
		}
		return parabolas;
	}

	public static Parabola[] CalculateFullTrajectory(DisplayParabolaRequestEvent request)
	{
		return CalculateFullTrajectory(request.startingPos, request.target, request.speed, request.arcHeight, request.xReductionPerBounce, request.bounces);
	}

	public static Parabola CalculateParabola(Vector3 startPos, Vector3 targetPos, float speed, float arcHeight)
	{
		Parabola parabola = new Parabola();
		parabola.points.Add(startPos);
		Vector3 transformPosition = startPos;
		Vector3 nextPos = new Vector3();
		while (nextPos != targetPos)
		{
			float x0 = startPos.x;
			float x1 = targetPos.x;
			float dist = x1 - x0;
			float nextX = Mathf.MoveTowards(transformPosition.x, x1, speed * Time.deltaTime);
			float baseY = Mathf.Lerp(startPos.y, targetPos.y, (nextX - x0) / dist);
			float arc = arcHeight * (nextX - x0) * (nextX - x1) / (-0.25f * dist * dist);
			nextPos = new Vector3(nextX, baseY + arc, transformPosition.z);
			parabola.points.Add(nextPos);
			transformPosition = nextPos;
		}
		return parabola;
	}

	public static int PointAmountInParabolaArray(Parabola[] parabolas)
	{
		int amount = 0;
		foreach(Parabola parabola in parabolas)
		{
			amount += parabola.points.Count;
		}
		return amount;
	}
}
