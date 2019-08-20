using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
public class CreateLevel
{
	[MenuItem("Assets/Create/Level")]
	public static Level Create(string name, int mapWidth, int mapHeight)
	{
		Level asset = ScriptableObject.CreateInstance<Level>();
		asset.Initialize(name, mapWidth, mapHeight);
		int sameNameIndex = 0;
		bool hasCycled = false;
		while (File.Exists(string.Format("Assets/Resources/Levels/{0}.asset", name)))
		{
			if (hasCycled) name = name.Remove(name.Length - 1);
			name = name + sameNameIndex;
			hasCycled = true;
			sameNameIndex++;
		}
		asset.levelName = name;
		AssetDatabase.CreateAsset(asset, string.Format("Assets/Resources/Levels/{0}.asset", name));
		AssetDatabase.SaveAssets();
		return asset;
	}
}