using UnityEngine;

public class MuzzleParticleController : MonoBehaviour
{
	public void Initialize(float duration)
	{
		transform.localPosition = Vector3.zero;
		transform.rotation = Quaternion.identity;
		Destroy(gameObject, 1f / (duration * GameManager.gameSpeedMultiplier));
	}
}
