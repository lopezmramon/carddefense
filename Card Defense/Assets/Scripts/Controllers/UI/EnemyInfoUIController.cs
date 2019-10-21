using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyInfoUIController : InfoUIController
{
	public TextMeshProUGUI enemyName, waveIndex, health, speed, reward, livesCost, specialAbilityText;
	public Sprite[] specialAbilityIcons;
	public Image specialAbility;
	public ScrollRect specialAbilityDisplay;
	public Button close;
	public UI.ThreeDimensional.UIObject3D display;
	private Enemy enemy;

	protected override void Awake()
	{
		base.Awake();
		specialAbility.gameObject.SetActive(false);
		close.onClick.AddListener(() =>
		{
			gameObject.SetActive(false);
		});
	}   

	protected virtual void OnEnable()
	{
		if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
		Vector3 mousePosition = Input.mousePosition;
		Vector3 calculatedPosition = mousePosition;
		calculatedPosition.x += rectTransform.rect.width / 2;
		calculatedPosition.y += rectTransform.rect.height / 2;
		rectTransform.position = calculatedPosition;
	}
	

	internal void Initialize(Enemy enemy)
	{
		this.enemy = enemy;
		enemy.OnHealthChanged += OnHealthChanged;
		SetupTexts();
		SetupDisplay();
	}

	private void OnHealthChanged()
	{
		UpdateCurrentHealthText();
	}

	private void SetupDisplay()
	{
		display.ObjectPrefab = enemy.transform;
		display.imageComponent.color = Color.white;
		display.UpdateDisplay();
	}

	private void UpdateCurrentHealthText()
	{
		health.text = string.Format("{0}/{1}", enemy.currentHealth, enemy.maxHealth);
	}

	private void SetupTexts()
	{
		UpdateCurrentHealthText();
		enemyName.text = enemy.enemyType.ToString();
		speed.text = enemy.speed.ToString();
		waveIndex.text = string.Format("{0} of {1}", (enemy.waveIndex+1).ToString(), enemy.totalWaveEnemies.ToString());
		reward.text = enemy.Reward.ToString();
		livesCost.text = enemy.LivesCost.ToString();
		specialAbilityText.text = string.Format("Special Ability: {0}", enemy.specialAbility.ToString());
	}
}
