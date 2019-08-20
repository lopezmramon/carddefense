using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
public class MakeLevel
{
	[MenuItem("Assets/Create/Level")]
	public static void CreateLevel()
	{
		string name = "Level";
		int mapWidth = 10;
		int mapHeight = 10;
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

		EditorUtility.FocusProjectWindow();

		Selection.activeObject = asset;
	}
}
