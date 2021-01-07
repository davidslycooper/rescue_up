using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	Transform target;
	bool targetFound;
	public float smooth;

	void Start () {
		Cursor.visible = false;
	}
	
	void FixedUpdate () {
		if (targetFound)
		{
			Vector3 pos = new Vector3(target.position.x, target.position.y, -20);
			transform.position = Vector3.Lerp(transform.position, pos, smooth);
		}
	}

	public void Configure(Transform configTarget)
    {
		target = configTarget;
		targetFound = true;
    }
}
