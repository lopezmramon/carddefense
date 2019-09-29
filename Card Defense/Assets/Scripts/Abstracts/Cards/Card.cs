using UnityEngine;
[System.Serializable]
public class Card
{
	public string name;
	public CardType cardType;
	public Element element;
	public string description;
	public int cost;
	public float duration;
	public PropertyModifier[] propertyModifiers;
	public float[] propertyModifierValues;
	public HandModifier[] handModifiers;
	public int[] handModifierValues;
	public string backgroundFileName, illustrationFileName, resourceFileName;
	public Sprite backgroundSprite, illustrationSprite, resourceSprite;

	public Card()
	{

	}
}
