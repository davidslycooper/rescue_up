using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyKill : MonoBehaviour
{
	int id;

	public void SetId(int newId)
    {
		id = newId;
    }

	void OnTriggerEnter2D (Collider2D other)
	{
		if (GameManager.pilot || !PhotonNetwork.InRoom)
		{
			if (other.tag == "Player")
			{
				if (other.gameObject.GetComponent<SubManager>() != null)
				{
					SubManager sub = other.gameObject.GetComponent<SubManager>();
					sub.Die(false, id);
					if (GameManager.tutorial)
					{
						PhotonNetwork.Destroy(gameObject);
					}
				}
			}
		}
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (GameManager.pilot || !PhotonNetwork.InRoom)
		{
			if (other.tag == "Player")
			{
				if (other.gameObject.GetComponent<SubManager>() != null)
				{
					SubManager sub = other.gameObject.GetComponent<SubManager>();
					sub.Die(false, id);
					if (GameManager.tutorial)
					{
						PhotonNetwork.Destroy(gameObject);
					}
				}
			}
		}
	}
}
