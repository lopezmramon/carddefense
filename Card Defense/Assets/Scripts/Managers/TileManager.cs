using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
	public GameObject voxelPrefab;
	public Material[] themeMaterials;
	public Color[] VFXColors;
	public Transform tileParent;
	private List<TileController> tiles = new List<TileController>();
	private TileController tileHovered;

	private void Awake()
	{
		themeMaterials = Resources.LoadAll<Material>("Materials/Voxel");
		CodeControl.Message.AddListener<GenerateTilesRequestEvent>(OnTilesGenerateRequested);
		CodeControl.Message.AddListener<TileVFXRequestEvent>(OnTileVFXRequested);
		CodeControl.Message.AddListener<CardDroppedEvent>(OnCardDropped);
		CodeControl.Message.AddListener<CardNoLongerOverTileEvent>(OnCardNoLongerOverTile);
		CodeControl.Message.AddListener<RemoveTileVFXRequestEvent>(OnRemoveTileVFXRequested);
	}

	private void OnCardDropped(CardDroppedEvent obj)
	{
		if(tileHovered != null)
		RemoveVFX(tileHovered);
	}

	private void OnCardNoLongerOverTile(CardNoLongerOverTileEvent obj)
	{
		RemoveVFX(tileHovered);
	}

	private void OnRemoveTileVFXRequested(RemoveTileVFXRequestEvent obj)
	{
		RemoveVFX(tileHovered);
	}

	private void OnTileVFXRequested(TileVFXRequestEvent obj)
	{
		tileHovered = FindTileControllerFromTile(obj.tile);
		ActivateVFX(tileHovered, obj.element);
	}

	private void OnTilesGenerateRequested(GenerateTilesRequestEvent obj)
	{
		StartCoroutine(GenerateLevel(obj.level));
		AstarPath.OnPostScan += OnPostScan;
	}

	private void OnPostScan(AstarPath script)
	{
		DispatchLevelReadyEvent(LevelManager.currentLevel);
	}

	private void ActivateVFX(TileController tileHovered, Element element)
	{
		tileHovered.ColorMaterial(VFXColors[(int)element]);
	}

	private void RemoveVFX(TileController tileHovered)
	{
		if (tileHovered == null) return;
		tileHovered.ResetColor();
	}

	private IEnumerator GenerateLevel(Level level)
	{
		Transform ground = tileParent.Find("Ground");
		ground.localScale = new Vector3(level.mapData.width * 2, level.mapData.height * 2, 1);
		ground.localPosition = new Vector3(level.mapData.width * 0.8f, 0, level.mapData.height * 0.8f);
		foreach (Tile tile in level.mapData.tiles)
		{
			if (tile.walkable)
			{

			}
			else if (!tile.IsEndingPoint && !tile.IsStartingPoint)
			{
				TileController newTile = Instantiate(voxelPrefab, tileParent).AddComponent<TileController>();
				tile.transform = newTile.transform;
				newTile.Initialize(tile, Instantiate(themeMaterials[(int)level.theme]));
				tiles.Add(newTile);
			}
		}
		yield return new WaitForSeconds(0.1f);
		AstarPath.active.Scan();
	}

	private TileController FindTileControllerFromTile(Tile tile)
	{
		return tiles.Find((x => x.tile == tile));
	}

	private void DispatchLevelReadyEvent(Level level)
	{
		CodeControl.Message.Send(new LevelReadyEvent(level));
	}
}
