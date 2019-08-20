using System.Collections.Generic;

[System.Serializable]
public class MapData
{
	public List<Tile> tiles;
	public int width, height;

	public MapData(List<Tile> tiles, int width, int height)
	{
		this.tiles = tiles;
		this.width = width;
		this.height = height;
	}
	public MapData(int width, int height)
	{
		tiles = new List<Tile>();
		this.width = width;
		this.height = height;
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				tiles.Add(new Tile(i, j));
			}
		}
	}

	public MapData()
	{
		tiles = new List<Tile>();
		for (int i = 0; i < 10; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				tiles.Add(new Tile(i, j));
			}
		}
		width = 10;
		height = 10;
	}
	public List<Tile> GetStartingPoints()
	{
		List<Tile> startingPoints = new List<Tile>();
		foreach (Tile tile in tiles)
		{
			if (tile.IsStartingPoint) startingPoints.Add(tile);
		}
		return startingPoints;
	}
	public List<Tile> GetEndingPoints()
	{
		List<Tile> endingPoints = new List<Tile>();
		foreach (Tile tile in tiles)
		{
			if (tile.IsEndingPoint) endingPoints.Add(tile);
		}
		return endingPoints;
	}

	public Tile FindTileByCoordinates(int x, int y)
	{
		return tiles.Find((t) => t.x == x && t.y == y);
	}
}
