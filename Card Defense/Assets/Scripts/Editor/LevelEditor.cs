using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class LevelEditor : EditorWindow
{
	public Level level;
	private int waveViewIndex = 0, enemyViewIndex = 0, mapWidth = 5, mapHeight = 5, lives, startingResource,
		selectedAbility, selectedType;
	public string widthInput = "1", heightInput = "1", newLevelName = string.Empty, livesInput = "1", startingResourceInput = "1";
	public EnemySpecialAbility specialAbility;
	public EnemyType enemyType;
	private Vector2 scrollPos;
	private int selectedTile;
	private Theme theme;
	private HashSet<Tile> startingTiles = new HashSet<Tile>(),
		endingTiles = new HashSet<Tile>(),
		pathTiles = new HashSet<Tile>();
	[MenuItem("Window/Level Editor %#e")]

	static void Init()
	{
		GetWindow(typeof(LevelEditor));
	}

	void OnEnable()
	{
		if (EditorPrefs.HasKey("ObjectPath"))
		{
			string objectPath = EditorPrefs.GetString("ObjectPath");
			level = AssetDatabase.LoadAssetAtPath(objectPath, typeof(Level)) as Level;
			if (level)
			{
				SetupLevelFromOpen();
			}
		}
	}

	void OnGUI()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label("Level Editor", EditorStyles.boldLabel);
		LevelCreationAndOpenButtons();
		if(level == null)
		newLevelName = GUILayout.TextField(newLevelName, 15, GUILayout.Width(150), GUILayout.Height(20));
		GUILayout.BeginHorizontal();
		GUILayout.Label("Map Width");
		GUILayout.Space(10);
		widthInput = GUILayout.TextField(widthInput, 2, GUILayout.Width(50), GUILayout.Height(20));
		if (!int.TryParse(widthInput, out mapWidth))
		{
			widthInput = "";
		}
		GUILayout.Label("Map Height");
		heightInput = GUILayout.TextField(heightInput, 2, GUILayout.Width(50), GUILayout.Height(20));
		if (!int.TryParse(heightInput, out mapHeight))
		{
			heightInput = "";
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(350);
		GUILayout.EndHorizontal();
		if (level != null)
		{
			newLevelName = level.levelName;
			GUILayout.BeginVertical();
			if (level != null)
				GUILayout.Label(string.Format("Currently Editing: {0}", level.levelName), EditorStyles.boldLabel);
			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
			GUILayout.Space(10);
			AddDeleteWaveButtons();
			GUILayout.Space(50);
			PreviousNextWaveButtons();
			GUILayout.Space(25);
			LivesResourcesAndThemeInput();
			GUILayout.EndHorizontal();
			GUILayout.Space(10);
			GUILayout.EndVertical();
			if (level.waves != null && level.waves.Count > 0)
			{
				GUILayout.BeginVertical();
				GUILayout.BeginHorizontal();
				waveViewIndex = Mathf.Clamp(EditorGUILayout.IntField("Current Wave", waveViewIndex, GUILayout.ExpandWidth(false)), 1, level.waves.Count);
				EditorGUILayout.LabelField("of   " + level.waves.Count.ToString() + "  waves", "", GUILayout.ExpandWidth(false));
				GUILayout.EndHorizontal();
				Wave wave = level.waves[waveViewIndex - 1];
				EnemyPropertyDropdowns();
				GUILayout.Space(10);
				GUILayout.BeginHorizontal();
				AddRemoveEnemyFromWaveButtons(wave);
				PreviousNextEnemyButtons(wave);
				GUILayout.Space(10);
				GUILayout.EndHorizontal();
				if (wave.enemies == null)
				{
					wave.enemies = new List<Enemy>();
				}
				EnemyCountDisplay(wave);
				EnemyDisplay(wave);
				GUILayout.EndVertical();
				GUILayout.Space(10);
				GUILayout.BeginHorizontal();
				TileEditingMap();
				GUILayout.Space(50);
				TileDataDisplay();
				GUILayout.EndHorizontal();
				GUILayout.Space(10);
			}
			else
			{
				GUILayout.Label("This Level has no waves.");
				GUILayout.Space(10);
			}
			GUILayout.FlexibleSpace();
			DeleteLevelButton();
		}
		if (GUI.changed && level != null)
		{
			EditorUtility.SetDirty(level);
		}
		GUIUtility.ExitGUI();
	}

	private void LivesResourcesAndThemeInput()
	{
		GUILayout.Label("Starting Lives: ", GUILayout.ExpandWidth(false));
		livesInput = GUILayout.TextField(livesInput, 3, GUILayout.Width(60), GUILayout.Height(20));
		if (!int.TryParse(livesInput, out lives))
		{
			livesInput = "";
		}
		level.lives = lives;
		GUILayout.Space(20);
		GUILayout.Label("Starting Resource: ", GUILayout.ExpandWidth(false));
		startingResourceInput = GUILayout.TextField(startingResourceInput, 4, GUILayout.Width(70), GUILayout.Height(20));
		if (!int.TryParse(startingResourceInput, out startingResource))
		{
			startingResourceInput = "";
		}
		level.startingTowerBuildingResource = startingResource;
		GUILayout.Space(20);
		GUILayout.Label("Level Theme: ", GUILayout.ExpandWidth(false));
		theme = (Theme)EditorGUILayout.EnumPopup(theme, GUILayout.ExpandWidth(false));
		level.theme = theme;
	}

	private void TileDataDisplay()
	{
		GUILayout.BeginHorizontal();
		GUILayout.BeginVertical();
		GUILayout.Label("Path Points: ");
		foreach (Tile tile in pathTiles)
		{
			GUILayout.Label(string.Format("Tile {0}, {1}", tile.x, tile.y));
		}
		GUILayout.EndVertical();
		GUILayout.BeginVertical();
		GUILayout.Label("Starting Points: ");
		foreach (Tile tile in startingTiles)
		{
			GUILayout.Label(string.Format("Tile {0}, {1}", tile.x, tile.y));
		}
		GUILayout.EndVertical();
		GUILayout.BeginVertical();
		GUILayout.Label("Ending Points: ");
		foreach (Tile tile in endingTiles)
		{
			GUILayout.Label(string.Format("Tile {0}, {1}", tile.x, tile.y));
		}
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
	}

	private void TileEditingMap()
	{
		if (level.mapData.width != mapWidth || level.mapData.height != mapHeight)
		{
			level.mapData = new MapData(mapWidth, mapHeight);
			pathTiles.Clear();
			startingTiles.Clear();
			endingTiles.Clear();
		}
		string[] tileStrings = new string[mapWidth * mapHeight];
		for (int i = mapHeight - 1; i >= 0; i--)
		{
			for (int j = mapWidth - 1; j >= 0; j--)
			{
				tileStrings[i * mapWidth + j] = string.Format("Tile {0}, {1}", j, i);
			}
		}
		selectedTile = GUILayout.SelectionGrid(selectedTile, tileStrings, mapWidth, GUILayout.ExpandWidth(false));
		Vector2Int coordinates = ArrayIndexTo2DArrayIndex(selectedTile, mapWidth, mapHeight);
		Tile tile = level.mapData.FindTileByCoordinates(coordinates.x, coordinates.y);
		GUILayout.Space(10);
		GUILayout.BeginVertical();
		GUILayout.Label(string.Format("Selected Tile {0}, {1}, which is {2} and {3}, {4}", coordinates.x, coordinates.y, tile.walkable ? "a part of the path" : "not walkable", tile.IsStartingPoint ? "a starting point" : "not a starting point", tile.IsEndingPoint ? "an ending point" : "not an ending point"));
		GUILayout.Space(10);
		GUILayout.BeginHorizontal();
		if (tile.walkable)
		{
			if (GUILayout.Button("Remove From Path"))
			{
				tile.walkable = false;
				pathTiles.Remove(tile);
			}
		}
		else
		{
			if (GUILayout.Button("Add to Path"))
			{
				tile.walkable = true;
				pathTiles.Add(tile);
			}
		}
		GUILayout.BeginVertical();
		if (tile.IsStartingPoint)
		{
			if (GUILayout.Button("Remove Starting Point"))
			{
				tile.IsStartingPoint = false;
				startingTiles.Remove(tile);
			}			
		}
		else
		{
			if (GUILayout.Button("Add Starting Point"))
			{
				tile.IsStartingPoint = true;
				startingTiles.Add(tile);
			}
		}
		if (tile.IsEndingPoint)
		{
			if (GUILayout.Button("Remove Ending Point"))
			{
				tile.IsEndingPoint = false;
				endingTiles.Remove(tile);
			}
		}
		else
		{
			if (GUILayout.Button("Add Ending Point"))
			{
				tile.IsEndingPoint = true;
				endingTiles.Add(tile);
			}
		}
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
	}

	private Vector2Int ArrayIndexTo2DArrayIndex(int index, int width, int height)
	{
		return new Vector2Int(index % width, index / width);
	}

	private void PreviousNextEnemyButtons(Wave wave)
	{
		if (wave == null || wave.enemies == null || wave.enemies.Count <= 1) return;
		if (GUILayout.Button("Prev Enemy", GUILayout.ExpandWidth(false)))
		{
			if (enemyViewIndex > 1)
				enemyViewIndex--;
		}
		GUILayout.Space(5);
		if (GUILayout.Button("Next Enemy", GUILayout.ExpandWidth(false)))
		{
			if (enemyViewIndex < wave.enemies.Count)
			{
				enemyViewIndex++;
			}
		}
	}

	private void EnemyDisplay(Wave wave)
	{
		scrollPos = GUILayout.BeginScrollView(scrollPos, false, false, GUILayout.ExpandWidth(true), GUILayout.Height(150));
		GUILayout.Space(5);
		for (int enemyIndex = 0; enemyIndex < wave.enemies.Count; enemyIndex++)
		{
			GUIStyle style = new GUIStyle(GUI.skin.label);
			bool isSelected = enemyViewIndex - 1 == enemyIndex;
			style.normal.textColor = isSelected ? Color.red : Color.black;
			GUILayout.Label(string.Format("{0}. Enemy: Type {1}, Special: {2}       ", enemyIndex + 1, wave.enemies[enemyIndex].enemyType.ToString(),
				wave.enemies[enemyIndex].specialAbility.ToString()), style);
			if (isSelected) SwitchEnemyPropertiesWithDropdowns(wave.enemies[enemyIndex]);
			GUILayout.Space(2);
		}
		GUILayout.EndScrollView();
	}

	private void SwitchEnemyPropertiesWithDropdowns(Enemy enemy)
	{
		string[] enemyTypeOptions = Enum.GetNames(typeof(EnemyType));
		enemy.enemyType = (EnemyType)EditorGUILayout.Popup("Enemy Type", (int)enemy.enemyType, enemyTypeOptions, GUILayout.ExpandWidth(false));
		string[] enemySpecialAbilityOptions = Enum.GetNames(typeof(EnemySpecialAbility));
		enemy.specialAbility = (EnemySpecialAbility)EditorGUILayout.Popup("Special Ability", (int)enemy.specialAbility, enemySpecialAbilityOptions, GUILayout.ExpandWidth(false));
	}

	private void EnemyPropertyDropdowns()
	{
		string[] enemyTypeOptions = Enum.GetNames(typeof(EnemyType));
		selectedType = EditorGUILayout.Popup("Enemy Type", selectedType, enemyTypeOptions, GUILayout.ExpandWidth(false));
		string[] enemySpecialAbilityOptions = Enum.GetNames(typeof(EnemySpecialAbility));
		selectedAbility = EditorGUILayout.Popup("Special Ability", selectedAbility, enemySpecialAbilityOptions, GUILayout.ExpandWidth(false));
	}

	private void EnemyCountDisplay(Wave wave)
	{
		if (wave.enemies.Count <= 0)
		{
			GUILayout.Label("This wave has no enemies");
		}
		else
		{
			GUILayout.BeginHorizontal();
			enemyViewIndex = Mathf.Clamp(EditorGUILayout.IntField("Current Enemy", enemyViewIndex, GUILayout.ExpandWidth(false)), 1, wave.enemies.Count);
			EditorGUILayout.LabelField("of   " + wave.enemies.Count.ToString() + "  enemies", "", GUILayout.ExpandWidth(false));
			GUILayout.EndHorizontal();
		}
	}

	private void PreviousNextWaveButtons()
	{
		if (level == null || level.waves == null || level.waves.Count <= 1) return;
		if (GUILayout.Button("Prev Wave", GUILayout.ExpandWidth(false)))
		{
			if (waveViewIndex > 1)
				waveViewIndex--;
		}
		GUILayout.Space(5);
		if (GUILayout.Button("Next Wave", GUILayout.ExpandWidth(false)))
		{
			if (waveViewIndex < level.waves.Count)
			{
				waveViewIndex++;
			}
		}
	}

	private void AddRemoveEnemyFromWaveButtons(Wave wave)
	{
		if (GUILayout.Button("Add Enemy", GUILayout.ExpandWidth(false)))
		{
			AddEnemy(wave);
		}
		if (wave.enemies.Count == 0) return;
		if (GUILayout.Button("Remove Enemy", GUILayout.ExpandWidth(false)))
		{
			DeleteEnemy(wave, enemyViewIndex - 1);
		}
	}

	private void DeleteEnemy(Wave wave, int enemyIndex)
	{
		if (enemyIndex < 0) return;
		wave.enemies.RemoveAt(enemyIndex);
	}

	private void AddEnemy(Wave wave)
	{
		enemyType = (EnemyType)selectedType;
		specialAbility = (EnemySpecialAbility)selectedAbility;
		wave.enemies.Add(new Enemy(enemyType, specialAbility));
	}

	private void AddDeleteWaveButtons()
	{
		if (GUILayout.Button("Add Wave", GUILayout.ExpandWidth(false)))
		{
			AddWave();
			EditorUtility.SetDirty(level);
		}
		if (level == null || level.waves == null || level.waves.Count == 0) return;
		if (GUILayout.Button("Delete Wave", GUILayout.ExpandWidth(false)))
		{
			DeleteWave(waveViewIndex - 1);
		}
	}

	private void DeleteLevelButton()
	{
		if (GUILayout.Button("Delete Level", GUILayout.ExpandWidth(false)))
		{
			DeleteLevel();
		}
	}

	private void DeleteLevel()
	{
		DestroyImmediate(level, true);
	}

	private void LevelCreationAndOpenButtons()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Space(10);
		if (GUILayout.Button("Create New Level", GUILayout.ExpandWidth(false)))
		{
			CreateNewLevel();
		}
		if (GUILayout.Button("Open Existing Level", GUILayout.ExpandWidth(false)))
		{
			OpenLevel();
		}
		GUILayout.EndHorizontal();
	}

	void CreateNewLevel()
	{
		waveViewIndex = 1;
		level = CreateLevel.Create(newLevelName, mapWidth, mapHeight);
		if (level)
		{
			//level.itemList = new List<InventoryItem>();
			string relPath = AssetDatabase.GetAssetPath(level);
			EditorPrefs.SetString("ObjectPath", relPath);
			startingTiles = new HashSet<Tile>();
			endingTiles = new HashSet<Tile>();
			foreach (Tile tile in level.mapData.tiles)
			{
				if (tile.IsEndingPoint) endingTiles.Add(tile);
				if (tile.IsStartingPoint) startingTiles.Add(tile);
			}
		}
	}

	void OpenLevel()
	{
		string absPath = EditorUtility.OpenFilePanel("Select Level", Application.dataPath + "Resources/Levels", "");
		if (absPath.StartsWith(Application.dataPath))
		{
			string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
			level = AssetDatabase.LoadAssetAtPath(relPath, typeof(Level)) as Level;
			if (level)
			{
				EditorPrefs.SetString("ObjectPath", relPath);
				SetupLevelFromOpen();
			}
		}
	}

	private void SetupLevelFromOpen()
	{
		startingTiles.Clear();
		endingTiles.Clear();
		pathTiles.Clear();
		mapWidth = level.mapData.width;
		widthInput = mapWidth.ToString();
		mapHeight = level.mapData.height;
		heightInput = mapHeight.ToString();
		livesInput = level.lives.ToString();
		theme = level.theme;
		startingResourceInput = level.startingTowerBuildingResource.ToString();
		foreach (Tile tile in level.mapData.tiles)
		{
			if (tile.IsEndingPoint) endingTiles.Add(tile);
			if (tile.IsStartingPoint) startingTiles.Add(tile);
			if (tile.walkable) pathTiles.Add(tile);
		}
	}

	void AddWave()
	{
		Wave newWave = new Wave();
		level.waves.Add(newWave);
		waveViewIndex = level.waves.Count;
	}

	void DeleteWave(int index)
	{
		level.waves.RemoveAt(index);
	}
}
