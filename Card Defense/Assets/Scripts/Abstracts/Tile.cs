using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class Tile
{
	public int x, y;
	public bool walkable;
	public bool IsStartingPoint;
	public bool IsEndingPoint;
	public Transform transform;

	public Tile(int x, int y, bool walkable, bool isStartingPoint, bool isEndingPoint)
	{
		this.x = x;
		this.y = y;
		this.walkable = walkable;
		IsStartingPoint = isStartingPoint;
		IsEndingPoint = isEndingPoint;
	}

	public Tile(int x, int y)
	{
		this.x = x;
		this.y = y;
		walkable = false;
		IsStartingPoint = false;
		IsEndingPoint = false;
	}

	public Vector3 Vector3FromCoordinates => new Vector3(x * 2, 0, y * 2);

	public bool CanBeBuiltOn()
	{
		return IsStartingPoint && IsEndingPoint && walkable;
	}
}
