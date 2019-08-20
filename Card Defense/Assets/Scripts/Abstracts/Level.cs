using System;
using System.Collections.Generic;
using UnityEngine;
public class Level : ScriptableObject
{
	public string levelName;
	public Theme theme;
	public MapData mapData;
	public List<Wave> waves;
	public int lives, startingTowerBuildingResource;

	public Level()
	{
	}

	public Level(string name, int mapWidth, int mapHeight)
	{
		levelName = name;
		mapData = new MapData(mapWidth, mapHeight);
		waves = new List<Wave>();
	}

	public void Initialize(string name, int mapWidth, int mapHeight)
	{
		levelName = name;
		mapData = new MapData(mapWidth, mapHeight);
		waves = new List<Wave>();
	}
	
	public int TotalEnemiesInLevel()
	{
		int enemies = 0;
		foreach(Wave wave in waves)
		{
			foreach(Enemy enemy in wave.enemies)
			{
				enemies++;
			}
		}
		return enemies;
	}
}
