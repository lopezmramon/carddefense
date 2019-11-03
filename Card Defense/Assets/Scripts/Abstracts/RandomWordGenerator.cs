using UnityEngine;
public static class RandomWordGenerator
{
	public static string CreateRandomWordNumberCombination()
	{
		//Dictionary of strings
		string[] words = { "Bold", "Think", "Friend", "Pony", "Fall", "Easy", "Powerful", "Strong" };
		//Random number from - to
		int randomNumber = Random.Range(2000, 3000);
		//Create combination of word + number
		string randomString = $"{words[Random.Range(0, words.Length)]}{randomNumber}";
		return randomString;
	}
}