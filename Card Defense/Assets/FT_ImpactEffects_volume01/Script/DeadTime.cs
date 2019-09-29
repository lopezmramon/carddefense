using UnityEngine;

public class DeadTime : MonoBehaviour
{
	public float deadTime;

	private void Awake()
	{
		Destroy(gameObject, deadTime);
	}
}
