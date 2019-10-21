using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InfoUIController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	protected Vector3 dragOffset;
	protected RectTransform rectTransform;

	protected virtual void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
	}
	
	public virtual void OnBeginDrag(PointerEventData eventData)
	{
		dragOffset = eventData.position - (Vector2)transform.position;
	}

	public virtual void OnDrag(PointerEventData eventData)
	{
		transform.position = eventData.position - (Vector2)dragOffset;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		//throw new NotImplementedException();
	}
}
