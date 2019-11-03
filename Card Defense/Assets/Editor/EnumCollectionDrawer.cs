using System;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Property Drawer for Enum Collection Type.
/// Displays a single entry of Type T for every value from enum TEnum.
/// Entry fields are labled with the toString of each value.
/// </summary>
/// <typeparam name="T">Type for entry fields.</typeparam>
/// <typeparam name="TEnum">enum for field lables.</typeparam>
public abstract class EnumCollectionDrawer<T,TEnum> : PropertyDrawer where T : class 
	where TEnum : struct, IConvertible, IComparable, IFormattable
{
    bool foldout;
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		var values = property.FindPropertyRelative("Collection");
		return !foldout ? EditorGUIUtility.singleLineHeight : values.arraySize * EditorGUIUtility.singleLineHeight + EditorGUIUtility.singleLineHeight;
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
		foldout = EditorGUI.Foldout(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), foldout, label);
        if(foldout)
		    for (var i = 0; i < values.arraySize; i++)
		    {
			    var pos = new Rect(position.x + 15, position.y + (EditorGUIUtility.singleLineHeight * (i + 1)), position.width - 15, EditorGUIUtility.singleLineHeight);
			    EditorGUI.PropertyField(pos, values.GetArrayElementAtIndex(i), new GUIContent(labels[i]));
		    }
	}
}
/// <summary>
/// Generic wrapper for editor because unity don't really like generics
/// </summary>
[CustomPropertyDrawer(typeof(Sample))]
public class SomethingCollectionDrawer : EnumCollectionDrawer<string, Defs> {}
