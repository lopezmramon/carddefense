﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoUIManager : MonoBehaviour
{
	public TowerInfoUIController towerInfoUIControllerPrefab;
	private TowerInfoUIController towerInfoUIController;
	public WaveInfoUIController waveInfoUIControllerPrefab;
	private WaveInfoUIController waveInfoUIController;
	public EnemyInfoUIController enemyInfoUIControllerPrefab;
	private EnemyInfoUIController enemyInfoUIController;
	public ContextMenuController contextMenuControllerPrefab;
	private ContextMenuController contextMenuController;
	private List<Wave> waves;

	private void Awake()
	{
		CodeControl.Message.AddListener<TowerInfoUIDisplayRequestEvent>(OnTowerInfoUIDisplayRequested);
		CodeControl.Message.AddListener<WaveInfoUIDisplayRequestEvent>(OnWaveInfoUIDisplayRequested);
		CodeControl.Message.AddListener<EnemyInfoUIDisplayRequestEvent>(OnEnemyInfoUIDisplayRequested);
		CodeControl.Message.AddListener<LevelReadyEvent>(OnLevelReady);
	}

	private void OnLevelReady(LevelReadyEvent obj)
	{
		waves = obj.level.waves;
	}

	private void OnTowerInfoUIDisplayRequested(TowerInfoUIDisplayRequestEvent obj)
	{
		GenerateTowerInfoUI(obj.tower);
	}

	private void OnWaveInfoUIDisplayRequested(WaveInfoUIDisplayRequestEvent obj)
	{
		GenerateWaveInfoUI();
	}

	private void OnEnemyInfoUIDisplayRequested(EnemyInfoUIDisplayRequestEvent obj)
	{
		GenerateEnemyInfoUI(obj.enemy);
	}

	private void GenerateEnemyInfoUI(Enemy enemy)
	{
		if(enemyInfoUIController == null)
		enemyInfoUIController = Instantiate(enemyInfoUIControllerPrefab, transform);
		enemyInfoUIController.gameObject.SetActive(true);
		enemyInfoUIController.Initialize(enemy);
	}

	private void GenerateWaveInfoUI()
	{
		if(waveInfoUIController == null)
		waveInfoUIController = Instantiate(waveInfoUIControllerPrefab, transform);
		waveInfoUIController.Initialize(waves);
	}

	private void GenerateTowerInfoUI(Tower tower)
	{
		if(towerInfoUIController == null)
		{
			towerInfoUIController = Instantiate(towerInfoUIControllerPrefab, transform);
		}
		towerInfoUIController.Initialize(tower);
	}
}
