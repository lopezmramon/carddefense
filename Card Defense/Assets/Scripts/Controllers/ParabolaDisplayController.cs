using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolaDisplayController : MonoBehaviour
{
	public LineRenderer lineRenderer;
	public Vector3 startingPos, endingPos;
	public float speed, arcHeight, xReductionPerBounce;
	public int bounces;
	private Transform target, projectileOrigin;
	private DisplayParabolaRequestEvent currentRequest;

	private void Awake()
	{
		CodeControl.Message.AddListener<DisplayParabolaRequestEvent>(OnDisplayParabolaRequested);
		CodeControl.Message.AddListener<StopDisplayingParabolaRequestEvent>(OnStopDisplayingParabolaRequested);
	}

	private void DisplayParabola(DisplayParabolaRequestEvent obj)
	{
		currentRequest = obj;
		currentRequest.speed = speed;
		currentRequest.arcHeight = arcHeight;
		currentRequest.xReductionPerBounce = xReductionPerBounce;
		currentRequest.bounces = bounces;
		if (projectileOrigin != obj.projectileOrigin) projectileOrigin = obj.projectileOrigin;
		if (obj.variableTarget != null)
		{
			target = obj.variableTarget;
			obj.target = target.position;
		}
		startingPos = projectileOrigin.position;
		lineRenderer.enabled = true;
		lineRenderer.positionCount = 0;
		Parabola[] parabolas = ParabolaCalculator.CalculateFullTrajectory(obj);
		lineRenderer.positionCount = ParabolaCalculator.PointAmountInParabolaArray(parabolas);
		int pointIndexInLineRenderer = 0;
		for (int parabolaIndex = 0; parabolaIndex < parabolas.Length; parabolaIndex++)
		{
			for (int pointIndex = 0; pointIndex < parabolas[parabolaIndex].points.Count; pointIndex++)
			{
				lineRenderer.SetPosition(pointIndexInLineRenderer, parabolas[parabolaIndex].points[pointIndex]);
				pointIndexInLineRenderer++;
			}
		}
		target.hasChanged = false;
	}

	private void Update()
	{
		if (target != null && target.hasChanged)
		{
			DisplayParabola(currentRequest);
		}
	}

	private void OnDisplayParabolaRequested(DisplayParabolaRequestEvent obj)
	{
		speed = obj.speed;
		arcHeight = obj.arcHeight;
		bounces = obj.bounces;
		xReductionPerBounce = obj.xReductionPerBounce;
		DisplayParabola(obj);
	}

	private void OnStopDisplayingParabolaRequested(StopDisplayingParabolaRequestEvent obj)
	{
		lineRenderer.enabled = false;
		target = null;
	}
}
