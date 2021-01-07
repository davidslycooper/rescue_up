using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RocketHit : MonoBehaviour
{
	public GameObject explosion;
	public AudioClip breakRock;

	StopProjectile stop;

	void Start () {
		stop = GetComponentInParent<StopProjectile> ();
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.layer == LayerMask.NameToLayer ("Shootable") || other.gameObject.layer == LayerMask.NameToLayer("Cave")) {
			stop.removeForce ();
			Instantiate (explosion, transform.position, transform.rotation);

			if (other.tag == "Enemy") {

				EnemyBehavior enemy = other.gameObject.GetComponent<EnemyBehavior>();

				if (enemy != null) {
					if (!GameManager.tutorial)
					{
						GameObject.Find("PlayerLog").GetComponent<PlayerLog>().EnemyHit(enemy.GetId(), other.gameObject.transform.position);
					}
					enemy.Hurt ();
				}
			} else if (other.tag == "BrokenRock")
			{
				if (!GameManager.tutorial)
				{
					GameObject.Find("PlayerLog").GetComponent<PlayerLog>().BrokenRock(other.gameObject.transform.position);
				}

				BreakRock(other.gameObject.transform.position);

				Destroy(other.gameObject);

				if (GameManager.tutorial)
				{
					GameObject.Find("PhotonTutorialGeneration").GetComponent<TutorialSteps>().RockHit();
				}
			}
			Destroy(gameObject);
		}
	}

	void OnTriggerStay2D (Collider2D other) {
		if (other.gameObject.layer == LayerMask.NameToLayer ("Shootable") || other.gameObject.layer == LayerMask.NameToLayer("Cave")) {
			stop.removeForce ();
			Instantiate (explosion, transform.position, transform.rotation);
			Destroy (gameObject);
			if (other.tag == "Enemy") {
				if (other.gameObject.GetComponent<EnemyBehavior> () != null) {
					EnemyBehavior hurt = other.gameObject.GetComponent<EnemyBehavior> ();
					hurt.Hurt ();
				}
			} else if (other.tag == "BrokenRock")
			{
				Destroy(other.gameObject);
			}
		}
	}

	void BreakRock(Vector3 pos)
    {
		GameObject obj = new GameObject("BreakRock");
		obj.transform.position = pos;

		AudioSource objAS = obj.AddComponent<AudioSource>();
		objAS.spatialBlend = 1;
		objAS.PlayOneShot(breakRock);

		Destroy(obj, breakRock.length);
	}
}
