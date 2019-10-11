using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUIController : MonoBehaviour
{
	public Dropdown resolutionsDropdown, windowModeDropdown, qualitySettingsDropdown;
	public Button resolutionButton, windowModeButton, qualityButton;

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
		resolutionButton.onClick.AddListener(() =>
		{
			SetResolution(resolutionsDropdown.value);
		});

		windowModeButton.onClick.AddListener(() =>
		{
			Screen.fullScreen = windowModeDropdown.value == 0;
			PlayerPrefs.SetInt("Fullscreen", windowModeDropdown.value);
		});

		qualitySettingsDropdown.onValueChanged.AddListener((value) =>
		{
			QualitySettings.SetQualityLevel(qualitySettingsDropdown.value);
			PlayerPrefs.SetInt("Quality", qualitySettingsDropdown.value);
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
		List<Dropdown.OptionData> resolutionDropdownOptions = new List<Dropdown.OptionData>();
		foreach (Resolution resolution in Screen.resolutions)
		{
			resolutionDropdownOptions.Add(new Dropdown.OptionData(resolution.ToString()));
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
		List<Dropdown.OptionData> windowModeDropdownOptions = new List<Dropdown.OptionData>();
		windowModeDropdownOptions.Add(new Dropdown.OptionData("Fullscreen"));
		windowModeDropdownOptions.Add(new Dropdown.OptionData("Windowed"));
		windowModeDropdown.AddOptions(windowModeDropdownOptions);
		windowModeDropdown.value = PlayerPrefs.GetInt("Fullscreen");
	}

	private void SetQualitySettingsDropdownOptions()
	{
		List<Dropdown.OptionData> qualitySettingsDropdownOptions = new List<Dropdown.OptionData>();
		foreach (string level in QualitySettings.names)
		{
			qualitySettingsDropdownOptions.Add(new Dropdown.OptionData(level));
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
}
