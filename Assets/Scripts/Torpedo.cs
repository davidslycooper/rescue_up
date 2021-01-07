using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torpedo : MonoBehaviour
{
	Vector3 direction;
	Rigidbody2D myRb; 

	Vector3 target;

	[Range(1, 10)]
	public int bulletSpeed;

	void Start() {
		myRb = GetComponent<Rigidbody2D>();

		target = transform.right;
	}
	
	void FixedUpdate () {
		direction = target * bulletSpeed;
		myRb.velocity = direction;
	}
}
