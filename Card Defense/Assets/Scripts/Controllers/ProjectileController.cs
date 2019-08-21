using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
	private ProjectileMover projectileMover;
	public List<Element> elements = new List<Element>();
	public float damage;

	public void Initialize(Transform target, List<Element> elements)
	{
		this.elements = elements;
		projectileMover = new ProjectileMover(transform, target, SpeedFromElements(), ElementUtility.MovementForElement(elements[0]));		
	}

	public void Initialize(Vector3 target, List<Element> elements)
	{
		this.elements = elements;	
		projectileMover = new ProjectileMover(transform, target, SpeedFromElements(), ElementUtility.MovementForElement(elements[0]));		
	}

	private void Update()
	{
		projectileMover.MoveAsRequired((success) =>
		{
			if (success)
			{
				projectileMover.target.GetComponent<EnemyController>().Damage(damage);
				Destroy(gameObject);
			}
			else
			{
				Destroy(gameObject);
			}
		});
	}

	private float SpeedFromElements()
	{
		return 10f;
	}
}
