using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class TowerInfoUIController : InfoUIController
{
	public Sprite[] propertyModifierIcons;
	public GameObject propertyModifierContainer;
	private Dictionary<PropertyModifier, GameObject> propertyModifierDisplays = new Dictionary<PropertyModifier, GameObject>();
	[Header("Tower Related")]
	public TextMeshProUGUI fireRate;
	public TextMeshProUGUI fireRateChange, fireRange, fireRangeChange, mainElement;
	public UI.ThreeDimensional.UIObject3D towerDisplay;
	public Sprite[] elementIcons;
	public Image[] elementDisplays;
	[Header("Projectile Related")]
	public TextMeshProUGUI bounces;
	public TextMeshProUGUI bouncesChange, chain, chainChange, aoe, aoeChange, damage, damageChange, slow, slowChange;
	public Image projectileDisplay;
	public Sprite[] projectileIcons;
	[Header("Modifier Related")]
	public ScrollRect modifiers;
	public Button sellButton, exitButton;
	public TextMeshProUGUI sellButtonText;
	private PropertyModifierHandler modifierHandler;
	private Tower tower;
	private Element[] elementsArray;

	protected override void Awake()
	{
		base.Awake();
		InitialSetup();
		CodeControl.Message.AddListener<SimulateUpgradeRequestEvent>(OnTowerUpgradeSimulationRequested);
		CodeControl.Message.AddListener<CardNoLongerOverTileEvent>(OnCardNoLongerOverTile);
		CodeControl.Message.AddListener<TowerUpgradedEvent>(OnTowerUpgraded);
		CodeControl.Message.AddListener<TowerModifierAppliedEvent>(OnTowerModifiersApplied);
	}

	private void OnCardNoLongerOverTile(CardNoLongerOverTileEvent obj)
	{
		ClearUpgradeTexts();
	}

	private void OnTowerModifiersApplied(TowerModifierAppliedEvent obj)
	{
		if (obj.handler != modifierHandler) return;
		foreach (PropertyModifier activeModifier in modifierHandler.activeModifiers)
		{
			propertyModifierDisplays[activeModifier].SetActive(true);
		}
	}

	private void InitialSetup()
	{
		exitButton.onClick.AddListener(() =>
		{
			gameObject.SetActive(false);
		});
		SetupModifiers();
	}

	private void OnTowerUpgraded(TowerUpgradedEvent obj)
	{
		if (obj.tower.transform.GetInstanceID() != tower.transform.GetInstanceID()) return;
		ClearUpgradeTexts();
		Initialize(obj.tower);
	}

	private void OnTowerUpgradeSimulationRequested(SimulateUpgradeRequestEvent obj)
	{
		if (obj.tower.transform.GetInstanceID() == tower.transform.GetInstanceID())
		{
			SetupUpgradeTexts(obj.element);
		}
	}

	private void SetupUpgradeTexts(Element element)
	{
		if (ElementUtility.ElementDamage(element) == 0) damageChange.text = string.Empty;
		else
		{
			damageChange.text = string.Format("{0}{1}", ElementUtility.ElementDamage(element) >= 0f ? "+" : "-", ElementUtility.ElementDamage(element).ToString());
			damageChange.color = ElementUtility.ElementDamage(element) >= 0f ? Color.green : Color.red;
		}
		if (ElementUtility.ElementRange(element) == 0) fireRangeChange.text = string.Empty;
		else
		{
			fireRangeChange.text = string.Format("{0}{1}", ElementUtility.ElementRange(element) >= 0f ? "+" : "-", ElementUtility.ElementRange(element).ToString());
			fireRangeChange.color = ElementUtility.ElementDamage(element) >= 0f ? Color.green : Color.red;
		}
		if (ElementUtility.ElementFireRate(element) == 0) fireRateChange.text = string.Empty;
		else
		{
			fireRateChange.text = string.Format("{0}{1}", ElementUtility.ElementFireRate(element) >= 0f ? "+" : "-", ElementUtility.ElementFireRate(element).ToString());
			fireRateChange.color = ElementUtility.ElementDamage(element) >= 0f ? Color.green : Color.red;
		}
		if (ElementUtility.ElementBounces(element) == 0) bounces.text = string.Empty;
		else
		{
			bouncesChange.text = string.Format("{0}{1}", ElementUtility.ElementBounces(element) >= 0f ? "+" : "-", ElementUtility.ElementBounces(element).ToString());
			bouncesChange.color = ElementUtility.ElementDamage(element) >= 0f ? Color.green : Color.red;
		}
		if (ElementUtility.ElementChain(element) == 0) chainChange.text = string.Empty;
		else
		{
			chainChange.text = string.Format("{0}{1}", ElementUtility.ElementChain(element) >= 0f ? "+" : "-", ElementUtility.ElementChain(element).ToString());
			chainChange.color = ElementUtility.ElementDamage(element) >= 0f ? Color.green : Color.red;
		}
		if (ElementUtility.ElementAoE(element) == 0) aoeChange.text = string.Empty;
		else
		{
			aoeChange.text = string.Format("{0}{1}", ElementUtility.ElementAoE(element) >= 0f ? "+" : "-", ElementUtility.ElementAoE(element).ToString());
			aoeChange.color = ElementUtility.ElementDamage(element) >= 0f ? Color.green : Color.red;
		}
		if (ElementUtility.ElementSlow(element) == 0) slowChange.text = string.Empty;
		else
		{
			slowChange.text = string.Format("{0}{1}%", ElementUtility.ElementSlow(element) >= 0f ? "+" : "-", ElementUtility.ElementSlow(element).ToString());
			slowChange.color = ElementUtility.ElementDamage(element) >= 0f ? Color.green : Color.red;
		}
	}

	private void ClearUpgradeTexts()
	{
		damageChange.text = string.Empty;
		fireRangeChange.text = string.Empty;
		fireRateChange.text = string.Empty;
		bouncesChange.text = string.Empty;
		chainChange.text = string.Empty;
		aoeChange.text = string.Empty;
		slowChange.text = string.Empty; 
	}

	internal void Initialize(Tower tower)
	{
		this.tower = tower;
		DeactivateAllDisplays();
		elementsArray = tower.elements.ToArray();
		modifierHandler = tower.modifierHandler;
		modifierHandler.OnPropertyReset += OnPropertyReset;
		modifierHandler.OnPropertyApplied += OnPropertyApplied;
		projectileDisplay.sprite = projectileIcons[(int)tower.elements.Peek()];
		SetupTexts();
		SetupDisplays();
		SetupButtons();
		gameObject.SetActive(true);
	}

	private void OnPropertyApplied(PropertyModifier propertyModifier)
	{
		propertyModifierDisplays[propertyModifier].SetActive(true);
	}

	private void OnPropertyReset(PropertyModifier propertyModifier)
	{
		propertyModifierDisplays[propertyModifier].SetActive(false);
	}

	private void SetupButtons()
	{
		sellButton.onClick.RemoveAllListeners();
		sellButton.onClick.AddListener(() =>
		{
			CodeControl.Message.Send(new SellTowerRequestEvent(tower.transform.GetComponent<TowerController>()));
			gameObject.SetActive(false);
		});
	}

	private void SetupTexts()
	{
		mainElement.text = tower.elements.Peek().ToString();
		fireRate.text = string.Format("{0}/s", ElementUtility.FireRate(elementsArray).ToString());
		fireRange.text = ElementUtility.TowerRange(elementsArray).ToString();
		bounces.text = ElementUtility.Bounces(elementsArray).ToString();
		chain.text = ElementUtility.ChainLength(elementsArray).ToString();
		slow.text = ElementUtility.Slow(elementsArray).ToString();
		aoe.text = ElementUtility.AoE(elementsArray).ToString();
		damage.text = ElementUtility.ProjectileDamage(elementsArray).ToString();
		sellButtonText.text = string.Format("Sell for: {0}", ElementUtility.SaleValue(elementsArray).ToString());
		ClearUpgradeTexts();
	}

	private void SetupDisplays()
	{
		towerDisplay.ObjectPrefab = tower.transform;
		towerDisplay.imageComponent.color = Color.white;
		towerDisplay.UpdateDisplay();
		for (int i = 0; i < elementsArray.Length; i++)
		{
			elementDisplays[i].sprite = elementIcons[(int)elementsArray[i]];
			elementDisplays[i].enabled = true;
		}
		for (int i = 0; i < tower.modifierHandler.activeModifiers.Count; i++)
		{
			propertyModifierDisplays[tower.modifierHandler.activeModifiers[i]].SetActive(true);
		}
	}

	private void SetupModifiers()
	{
		for (int i = 0; i < propertyModifierIcons.Length; i++)
		{
			GameObject container = Instantiate(propertyModifierContainer, modifiers.content);
			container.GetComponent<Image>().sprite = propertyModifierIcons[i];
			propertyModifierDisplays.Add((PropertyModifier)i, container);
			container.SetActive(false);
		}
	}

	private void DeactivateAllDisplays()
	{
		foreach (Image display in elementDisplays)
		{
			display.enabled = false;
		}
		foreach (KeyValuePair<PropertyModifier, GameObject> propertyModifierDisplay in propertyModifierDisplays)
		{
			propertyModifierDisplay.Value.SetActive(false);
		}
	}
}
