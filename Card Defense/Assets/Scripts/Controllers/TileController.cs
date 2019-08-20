using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public Tile tile;
	private new Renderer renderer;
	private bool hovered = false;

	private void Awake()
	{
		renderer = GetComponent<Renderer>();
	}

	public void Initialize(Tile tile, Material material)
	{
		if (renderer == null) renderer = GetComponent<Renderer>();
		this.tile = tile;
		name = string.Format("Tile {0}, {1}", tile.x, tile.y);
		transform.localPosition = new Vector3(tile.x * 2, 0, tile.y * 2);
		renderer.material = material;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		hovered = true;
		DispatchTileEnteredEvent();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		hovered = false;
		DispatchTileExitEvent();
	}

	private void Update()
	{
		if (hovered && Input.GetAxis("Fire2") != 0)
		{
			DispatchContextMenuRequestEvent();
		}
	}

	private void DispatchTileEnteredEvent()
	{
		CodeControl.Message.Send(new CursorEnterTileEvent(tile));
	}

	private void DispatchTileExitEvent()
	{
		CodeControl.Message.Send(new CursorExitTileEvent(tile));
	}

	private void DispatchContextMenuRequestEvent()
	{
		CodeControl.Message.Send(new ContextMenuRequestEvent(tile));
	}

	internal void ColorMaterial(Color color)
	{
		renderer.material.color = color;
	}

	internal void ResetColor()
	{
		renderer.material.color = Color.white;
	}
}

