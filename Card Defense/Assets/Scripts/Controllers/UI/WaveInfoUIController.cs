using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WaveInfoUIController : InfoUIController
{
	public Image background;
	public TextMeshProUGUI title, enemyAmount, totalReward;
	public ScrollRect enemyDisplayRect;
	public GameObject object3DPrefab;
	public EnemyCollection enemyCollection;
	private List<Wave> waves;
	public Button previousWave, nextWave, close;
	private int currentWave;

	protected override void Awake()
	{
		base.Awake();
		SetupButtons();
	}

	private void SetupButtons()
	{
		previousWave.onClick.AddListener(() =>
		{
			ChangeDisplayedWave(-1);
		});

		nextWave.onClick.AddListener(() =>
		{
			ChangeDisplayedWave(1);
		});

		close.onClick.AddListener(() =>
		{
			ToggleDisplays(false);
			ClearEnemyDisplay();
		});
	}

	private void ToggleDisplays(bool active)
	{
		/*title.gameObject.SetActive(active);
		enemyAmount.gameObject.SetActive(active);
		totalReward.gameObject.SetActive(active);
		background.gameObject.SetActive(false);*/
		gameObject.SetActive(active);
	}

	private void ClearEnemyDisplay()
	{
		foreach (Transform child in enemyDisplayRect.content)
		{
			Destroy(child.gameObject);
		}
	}

	private void ChangeDisplayedWave(int change)
	{
		if (currentWave + change >= waves.Count)
		{
			currentWave = 0;
		}
		else if (currentWave + change < 0)
		{
			currentWave = waves.Count - 1;
		}
		else
		{
			currentWave += change;
		}
		SetDisplay(waves[currentWave]);
	}

	private void SetDisplay(Wave wave)
	{
		ClearEnemyDisplay();
		ToggleDisplays(true);
		title.text = string.Format("Wave {0} of {1}", currentWave + 1, waves.Count);
		enemyAmount.text = $"Enemies in Wave: {wave.enemies.Count}";
		float totalRewardInWave = 0;
		Dictionary<EnemyType, int> amountOfEnemiesPerType = new Dictionary<EnemyType, int>();
		foreach (Enemy enemy in wave.enemies)
		{
			totalRewardInWave += enemy.Reward;
			if (amountOfEnemiesPerType.ContainsKey(enemy.enemyType))
			{
				amountOfEnemiesPerType[enemy.enemyType]++;
			}
			else
			{
				amountOfEnemiesPerType.Add(enemy.enemyType, 1);
			}
		}
		totalReward.text = $"Total Reward: {totalRewardInWave}";
		foreach (KeyValuePair<EnemyType, int> enemyType in amountOfEnemiesPerType)
		{
			GameObject enemyDisplay = Instantiate(object3DPrefab, enemyDisplayRect.content);
			UI.ThreeDimensional.UIObject3D object3D = enemyDisplay.GetComponent<UI.ThreeDimensional.UIObject3D>();
			object3D.ObjectPrefab = enemyCollection[enemyType.Key];
			object3D.imageComponent.color = Color.white;
			enemyDisplay.GetComponentInChildren<TextMeshProUGUI>().text = $"{enemyType.Key} x {enemyType.Value}";
		}
	}

	internal void Initialize(List<Wave> waves)
	{
		//transform.position = Vector3.zero;
		this.waves = waves;
		currentWave = WaveManager.currentWaveIndex >= 0 ? WaveManager.currentWaveIndex : 0;
		Wave wave = waves[currentWave];
		ToggleDisplays(true);
		SetDisplay(wave);
	}
}
