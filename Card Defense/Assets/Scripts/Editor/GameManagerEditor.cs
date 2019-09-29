using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		GameManager gameManager = (GameManager)target;
		gameManager.speed = EditorGUILayout.FloatField("Game Speed", gameManager.speed);
		if (GUILayout.Button("Update Game Speed"))
		{
			gameManager.UpdateGameSpeed(gameManager.speed);
		}
	}
}
