using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignHit : MonoBehaviour
{
	StopProjectile stop;

	// Use this for initialization
	void Start()
	{
		stop = GetComponentInParent<StopProjectile>();
	}

	// Update is called once per frame
	void Update()
	{

	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Shootable") || other.gameObject.layer == LayerMask.NameToLayer("Cave"))
		{
			stop.removeForce();
		}
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Shootable") || other.gameObject.layer == LayerMask.NameToLayer("Cave"))
		{
			stop.removeForce();
		}
	}
}