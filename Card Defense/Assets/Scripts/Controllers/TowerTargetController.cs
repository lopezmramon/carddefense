﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerTargetController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
	private new Renderer renderer;
	private MeshCollider meshCollider;

	public void Initialize(Vector3 targetLocation, Element[] elements)
	{
		renderer = GetComponent<Renderer>();
		meshCollider = GetComponent<MeshCollider>();
		Deactivate();
		PositionOnPath(targetLocation);		
	}

	private void PositionOnPath(Vector3 targetLocation)
	{
		while (Physics.OverlapSphere(targetLocation, 0.1f).Length > 0)
		{
			targetLocation.y += 0.1f;
		}
		transform.position = targetLocation;
	}

	public void Deactivate()
	{
		renderer.enabled = false;
		meshCollider.enabled = false;
		transform.hasChanged = false;
	}

	public void Activate()
	{
		renderer.enabled = true;
		meshCollider.enabled = true;
		transform.hasChanged = false;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		meshCollider.enabled = false;
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (eventData.button != PointerEventData.InputButton.Left) return;
		if (eventData.pointerCurrentRaycast.gameObject == null)
		{

		}
		else
		{
			Transform currentRaycastResult = eventData.pointerCurrentRaycast.gameObject.transform;
			Vector3 mousePosition = eventData.pointerCurrentRaycast.worldPosition;
			mousePosition.y += 0.1f;
			transform.position = mousePosition;
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (gameObject.activeInHierarchy)
			meshCollider.enabled = true;
		transform.hasChanged = false;
	}
}
