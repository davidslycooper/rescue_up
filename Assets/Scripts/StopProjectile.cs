using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopProjectile : MonoBehaviour {

	Rigidbody2D myRb;

	void Start () {
		myRb = GetComponent<Rigidbody2D> ();
	}

	public void removeForce() {
		myRb.velocity = new Vector2 (0, 0);
		if (GetComponent<Torpedo>() != null)
		{
			Destroy(GetComponent<Torpedo>());
		} else if (GetComponent<Flare>() != null)
		{
			Destroy(GetComponent<Flare>());
		}
	}
}
