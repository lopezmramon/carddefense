using System;
using UnityEditor;
using UnityEngine;

//drawer work is done here. This is generic but unity is not a fan of generics so we have to declare it in a wrapper to use this.
public abstract class EnumCollectionDrawer<T, TEnum> : PropertyDrawer where T : class
	where TEnum : struct, IConvertible, IComparable, IFormattable
{
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		var values = property.FindPropertyRelative("Collection");
		return values.arraySize * EditorGUIUtility.singleLineHeight + EditorGUIUtility.singleLineHeight;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		var labels = Enum.GetNames(typeof(TEnum));
		var values = property.FindPropertyRelative("Collection");

		for (var i = values.arraySize; i < labels.Length; i++)
			values.InsertArrayElementAtIndex(i);

		if (values.arraySize > labels.Length)
			values.arraySize = labels.Length;

		values.serializedObject.ApplyModifiedProperties();
		EditorGUI.LabelField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), label);
		for (var i = 0; i < values.arraySize; i++)
		{
			var pos = new Rect(position.x + 15, position.y + (EditorGUIUtility.singleLineHeight * (i + 1)), position.width - 15, EditorGUIUtility.singleLineHeight);
			EditorGUI.PropertyField(pos, values.GetArrayElementAtIndex(i), new GUIContent(labels[i]));
		}
	}
}

[CustomPropertyDrawer(typeof(LocalizeCollection))]
public class SomethingCollectionDrawer : EnumCollectionDrawer<GameObject, LocalizeDefs> { }
[CustomPropertyDrawer(typeof(ElementSpriteCollection))]
public class ElementSpriteCollectionDrawer : EnumCollectionDrawer<Sprite, Element> { }
[CustomPropertyDrawer(typeof(ElementTowerCollection))]
public class ElementTowerCollectionDrawer : EnumCollectionDrawer<TowerController, Element> { }
[CustomPropertyDrawer(typeof(ProjectileCollection))]
public class ProjectileCollectionDrawer : EnumCollectionDrawer<ProjectileController, Element> { }
