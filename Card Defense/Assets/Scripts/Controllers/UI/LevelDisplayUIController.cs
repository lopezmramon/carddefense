using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelDisplayUIController : MonoBehaviour
{
	public Button button;
	public TextMeshProUGUI levelNameText;
	private int index;
	private Outline outline;
	public void Initialize(Sprite sprite, string levelName, int index, System.Action<int> callback)
	{
		button.image.sprite = sprite;
		outline = button.image.gameObject.AddComponent<Outline>();
		outline.effectColor = Color.black;
		outline.effectDistance = new Vector2(6, 6);
		outline.enabled = false;
		levelNameText.text = levelName;
		button.onClick.AddListener(() =>
		{
			callback(index);
		}
		);
	}

	public void ToggleOutline(bool active)
	{
		outline.enabled = active;
	}
}
