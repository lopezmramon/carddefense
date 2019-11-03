using UnityEngine;

[System.Serializable]
public class Card
{
	public int index;
	public string name;
	public CardType cardType;
	public Element element;
	public string description;
	public int cost;
	public float duration;
	public PropertyModifier[] propertyModifiers;
	public float[] propertyModifierValues;
	public HandModifier handModifier;
	public int handModifierValue;
	public ExtraModifier extraModifier;
	public int extraModifierValue;
	public string backgroundFileName, illustrationFileName, resourceFileName;
	public Sprite backgroundSprite, illustrationSprite, resourceSprite;

	public Card()
	{

	}
}
