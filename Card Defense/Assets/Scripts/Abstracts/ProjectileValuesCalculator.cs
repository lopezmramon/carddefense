using System.Collections.Generic;

public static class ProjectileValuesCalculator 
{
	public static float SpeedFromElements(Queue<Element> elements)
	{
		return 8f;
	}

	public static float LandingAOEFromElements(Queue<Element> elements)
	{
		return 3f;
	}

	public static int BouncesFromElements(Queue<Element> elements)
	{
		return 3;
	}
}
