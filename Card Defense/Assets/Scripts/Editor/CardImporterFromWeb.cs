using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
#if UNITY_5_3_OR_NEWER
using UnityEngine.Networking;
#else
using UnityEngine.Experimental.Networking;
#endif
using UnityEditor;
#endif

#if UNITY_EDITOR
public class CardImporterFromWeb : Editor
{
	[MenuItem("Tools/Import Cards")]
	static void Init()
	{
		string url = "https://docs.google.com/spreadsheets/d/e/2PACX-1vQND0nii0q-kTGGvhw_2zFod78Fo2BXQx821K8QBY7W1n-90fn4doRETuO9Ch7SloyfM1_1f9pKWGJR/pub?gid=0&single=true&output=csv";
		string assetfile = "Assets/Resources/Cards/cards.asset";

		StartCoroutine(DownloadAndImport(url, assetfile));
	}

	static IEnumerator DownloadAndImport(string url, string assetfile)
	{
		//WWWForm form = new WWWForm();
		//UnityWebRequest www = UnityWebRequest.Post(url, form);
		UnityWebRequest www = UnityWebRequest.Get(url);
		yield return www.SendWebRequest();

		while (www.isDone == false)
		{
			yield return new WaitForEndOfFrame();
		}

		if (www.error != null)
		{
			Debug.Log("UnityWebRequest.error:" + www.error);
		}
		else if (www.downloadHandler.text == "" || www.downloadHandler.text.IndexOf("<!DOCTYPE") != -1)
		{
			Debug.Log("Unknown Format:" + www.downloadHandler.text);
		}
		else
		{
			ImportCardData(www.downloadHandler.text, assetfile);
#if DEBUG_LOG || UNITY_EDITOR
			Debug.Log("Imported Asset: " + assetfile);
#endif
		}
	}

	public static void ImportCardData(string text, string assetfile)
	{
		List<string[]> rows = CSVSerializer.ParseCSV(text);
		if (rows != null)
		{
			CardData cardData = AssetDatabase.LoadAssetAtPath<CardData>(assetfile);
			if (cardData == null)
			{
				cardData =(CardData) ScriptableObject.CreateInstance(typeof(CardData));
				AssetDatabase.CreateAsset(cardData, assetfile);
			}
			cardData.cards = CSVSerializer.Deserialize<Card>(rows);
			EditorUtility.SetDirty(cardData);
			AssetDatabase.SaveAssets();
		}
	}

	static void StartCoroutine(IEnumerator routine)
	{
		_coroutine.Add(routine);
		if (_coroutine.Count == 1)
			EditorApplication.update += ExecuteCoroutine;
	}
	static List<IEnumerator> _coroutine = new List<IEnumerator>();
	static void ExecuteCoroutine()
	{
		for (int i = 0; i < _coroutine.Count;)
		{
			if (_coroutine[i] == null || !_coroutine[i].MoveNext())
				_coroutine.RemoveAt(i);
			else
				i++;
		}
		if (_coroutine.Count == 0)
			EditorApplication.update -= ExecuteCoroutine;
	}
}
#endif