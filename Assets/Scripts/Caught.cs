using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caught : MonoBehaviour
{
	public int mineralType;
	public int mineralInd;

	EntityManager entityMan;
	PlayerController sub;

	bool caught;

	void Start()
    {
		if (!GameManager.pilot)
		{
			this.enabled = false;
		}
		entityMan = transform.root.gameObject.GetComponent<EntityManager>();
    }

	void Update()
    {
		if (caught)
        {
			if (sub.IsRetracted())
			{
				transform.parent.gameObject.GetComponent<BoxCollider2D>().enabled = true;

				entityMan.CatchMineral(mineralInd, mineralType);
			}
        }
    }

	public void SetMineralIndex(int mineralIndex)
    {
		mineralInd = mineralIndex;
    }

	void OnTriggerEnter2D(Collider2D other)
	{
		if (!caught && other.gameObject.tag == "Claw")
		{
			sub = other.transform.root.gameObject.GetComponent<PlayerController>();

			if (!sub.IsRetracted())
			{
				transform.parent = other.transform;
				transform.parent.gameObject.GetComponent<BoxCollider2D>().enabled = false;
				GetComponent<CircleCollider2D>().enabled = false;
				caught = true;
			}
		}
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (!caught && other.gameObject.tag == "Claw")
		{
			sub = other.transform.root.gameObject.GetComponent<PlayerController>();

			if (!sub.IsRetracted())
			{
				transform.parent = other.transform;
				transform.parent.gameObject.GetComponent<BoxCollider2D>().enabled = false;
				GetComponent<CircleCollider2D>().enabled = false;
				caught = true;
			}
		}
	}
}
