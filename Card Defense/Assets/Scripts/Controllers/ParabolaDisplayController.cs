using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolaDisplayController : MonoBehaviour
{
	public LineRenderer lineRenderer;

	private void Awake()
	{
		CodeControl.Message.AddListener<DisplayParabolaRequestEvent>(OnDisplayParabolaRequested);
		CodeControl.Message.AddListener<StopDisplayingParabolaRequestEvent>(OnStopDisplayingParabolaRequested);
	}

	private void OnDisplayParabolaRequested(DisplayParabolaRequestEvent obj)
	{
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
	}

	private void OnStopDisplayingParabolaRequested(StopDisplayingParabolaRequestEvent obj)
	{
		lineRenderer.enabled = false;
	}
}
