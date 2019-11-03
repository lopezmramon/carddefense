using System.Collections.Generic;
[System.Serializable]
public class CardDeck 
{
	public List<int> cardIndexes = new List<int>();
	public string name;

	public CardDeck(List<int> cardIndexes)
	{
		this.cardIndexes = cardIndexes;
	}
}
