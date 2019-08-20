using UnityEngine;
[System.Serializable]
public class Card
{
	public string name;
	public CardType cardType;
	public Element element;
	public string description;
	public int cost, time;
	public string backgroundFileName, illustrationFileName, resourceFileName;
	public Sprite backgroundSprite, illustrationSprite, resourceSprite;

	public Card()
	{

	}

	public Card(string name, CardType cardType, string backgroundFileName, string illustrationFilename, string resourceFileName, string description, int cost)
	{
		this.name = name;
		this.cardType = cardType;
		this.backgroundFileName = backgroundFileName;
		this.illustrationFileName = illustrationFilename;
		this.resourceFileName = resourceFileName;
		this.description = description;
		this.cost = cost;
	}
}
