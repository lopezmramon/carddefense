using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SettingsUIController : MonoBehaviour
{
	public TMP_Dropdown resolutionsDropdown, windowModeDropdown, qualitySettingsDropdown;
	public Button resolution, windowMode, quality, close;

	private void Awake()
	{
		SetResolutionsDropdownOptions();
		SetWindowModeDropdownOptions();
		SetQualitySettingsDropdownOptions();
		SetupButtons();
	}

	private void Start()
	{
		CheckPlayerPrefs();
	}

	private void CheckPlayerPrefs()
	{
		if (PlayerPrefs.HasKey("Resolution"))
			SetResolution(PlayerPrefs.GetInt("Resolution"));
		if (PlayerPrefs.HasKey("Quality"))
			QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("Quality"));
		if (PlayerPrefs.HasKey("Fullscreen"))
			Screen.fullScreen = PlayerPrefs.GetInt("Fullscreen") == 0;
	}

	private void SetupButtons()
	{
		resolution.onClick.AddListener(() =>
		{
			SetResolution(resolutionsDropdown.value);
		});

		windowMode.onClick.AddListener(() =>
		{
			Screen.fullScreen = windowModeDropdown.value == 0;
			PlayerPrefs.SetInt("Fullscreen", windowModeDropdown.value);
		});

		quality.onClick.AddListener(() =>
		{
			QualitySettings.SetQualityLevel(qualitySettingsDropdown.value);
			PlayerPrefs.SetInt("Quality", qualitySettingsDropdown.value);
		});

		close.onClick.AddListener(() =>
		{
			DispatchChangeViewRequestEvent();
		});
	}

	private void SetResolution(int index)
	{
		Resolution resolution = Screen.resolutions[index];
		if (Screen.currentResolution.width != resolution.width ||
		Screen.currentResolution.height != resolution.height ||
		Screen.currentResolution.refreshRate != resolution.refreshRate)
		{
			Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
		}
		PlayerPrefs.SetInt("Resolution", resolutionsDropdown.value);
	}

	private void SetResolutionsDropdownOptions()
	{
		List<TMP_Dropdown.OptionData> resolutionDropdownOptions = new List<TMP_Dropdown.OptionData>();
		foreach (Resolution resolution in Screen.resolutions)
		{
			resolutionDropdownOptions.Add(new TMP_Dropdown.OptionData(resolution.ToString()));
		}
		resolutionsDropdown.AddOptions(resolutionDropdownOptions);
		if (PlayerPrefs.HasKey("Resolution"))
		{
			resolutionsDropdown.value = PlayerPrefs.GetInt("Resolution");
		}
		else
		{
			for (int i = 0; i < resolutionDropdownOptions.Count; i++)
			{
				if (resolutionDropdownOptions[i].text.Contains(Screen.currentResolution.ToString()))
				{
					resolutionsDropdown.value = i;
				}
			}
		}
	}

	private void SetWindowModeDropdownOptions()
	{
		List<TMP_Dropdown.OptionData> windowModeDropdownOptions = new List<TMP_Dropdown.OptionData>();
		windowModeDropdownOptions.Add(new TMP_Dropdown.OptionData("Fullscreen"));
		windowModeDropdownOptions.Add(new TMP_Dropdown.OptionData("Windowed"));
		windowModeDropdown.AddOptions(windowModeDropdownOptions);
		windowModeDropdown.value = PlayerPrefs.GetInt("Fullscreen");
	}

	private void SetQualitySettingsDropdownOptions()
	{
		List<TMP_Dropdown.OptionData> qualitySettingsDropdownOptions = new List<TMP_Dropdown.OptionData>();
		foreach (string level in QualitySettings.names)
		{
			qualitySettingsDropdownOptions.Add(new TMP_Dropdown.OptionData(level));
		}
		qualitySettingsDropdown.AddOptions(qualitySettingsDropdownOptions);
		if (PlayerPrefs.HasKey("Quality"))
		{
			qualitySettingsDropdown.value = PlayerPrefs.GetInt("Quality");
		}
		else
		{
			qualitySettingsDropdown.value = (int)QualitySettings.GetQualityLevel();
		}
	}

	private void DispatchChangeViewRequestEvent()
	{
		CodeControl.Message.Send(new ChangeViewRequestEvent(View.Settings, View.Back, true));
	}
}
