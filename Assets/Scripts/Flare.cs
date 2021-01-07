using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flare : MonoBehaviour
{
	Vector3 direction;
	Rigidbody2D myRb;

	Vector3 target;

	float energy;

	[Range(1, 10)]
	public int bulletSpeed;

	void Start()
	{
		myRb = GetComponent<Rigidbody2D>();
		energy = 1;
	}

	void FixedUpdate()
	{
		direction = transform.right * 1f;
		myRb.velocity = direction * bulletSpeed * energy;
		myRb.AddTorque(bulletSpeed * 10 * energy);
		energy = Mathf.Max(energy - 0.01f, 0);
	}
}
