using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerTargetController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
	private new Renderer renderer;
	private MeshCollider meshCollider;
	private float maxDistance;
	public void SetMaxDistance(float distance) => maxDistance = distance * 1.5f;
	private Vector3 towerLocation;

	public void Initialize(Vector3 targetLocation, Vector3 towerLocation, float maxDistance, Element[] elements)
	{
		renderer = GetComponent<Renderer>();
		meshCollider = GetComponent<MeshCollider>();
		this.towerLocation = towerLocation;
		this.maxDistance = maxDistance * 1.5f;
		Deactivate();
		PositionOnPath(targetLocation);
	}

	private void PositionOnPath(Vector3 targetLocation)
	{
		targetLocation.x += 0.5f;
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
			if (Vector3.Distance(mousePosition, towerLocation) > maxDistance) return;
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
