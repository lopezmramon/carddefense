using UnityEngine;

public class ChargeParticleController : MonoBehaviour
{
	public OrbitMovement orbitMovement;
	private MagicLightFlicker lightFlicker;

	public void Initialize(Transform rotationTarget, OrbitMovement orbitMovement)
	{
		this.orbitMovement = orbitMovement;
		lightFlicker = GetComponentInChildren<MagicLightFlicker>();
		if (lightFlicker != null) lightFlicker.Toggle(false);
	}

	private void Update()
	{
		orbitMovement.Rotate();
	}
}
