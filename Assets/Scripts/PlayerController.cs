using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerController : MonoBehaviour
{
	bool tutorial;

	public float maxSpeed;

	Rigidbody2D myRb;
	Animator myAnim;
	SubManager subMan;

	int state; // 0: Apanhar; 1: Sinalizar; 2: Disparar; 3: Poupança de Energia

	bool facingRight;

	public GameObject crossPrefab;
	GameObject cross;

	GameObject map;

	public float bulletSpeed;

	Vector3 target;
	Vector3 shooter;

	[Range(0.1f,10)]
	public float fireRate;
	float nextFire = 0f;

	CameraShake cameraShake;

	float centerY;

	public GameObject handPrefab;
	public GameObject signerPrefab;
	public GameObject bottomCannonPrefab;
	public GameObject topCannonPrefab;

	GameObject hand;
	GameObject claw;
	GameObject midArm1;
	GameObject midArm2;
	GameObject botArm;
	GameObject signer;
	GameObject signLight;
	GameObject signSound;
	GameObject bottomCannon;
	GameObject topCannon;

	int arms = 1;
	bool retracted;

	bool caught;

	GameObject[][] items;

	public Vector3 handPos;
	public Vector3 signerPos;
	public Vector3 bottomCannonPos;
	public Vector3 topCannonPos;

	bool transition;

	int sonar;

	PassiveSonar passiveSonar;
	ActiveSonar activeSonar;

	PhotonView PV;

	void Start () {
		myRb = GetComponent<Rigidbody2D> ();
		myAnim = GetComponent<Animator> ();
		subMan = GetComponent<SubManager>();
		cameraShake = Camera.main.GetComponent<CameraShake> ();
		map = GameObject.FindWithTag("Map");

		PV = GetComponent<PhotonView>();

		facingRight = false;
		state = 3;

		cross = Instantiate (crossPrefab, transform.position, Quaternion.Euler (new Vector3 (0, 0, 0)));

		centerY = Screen.height / 2;

		items = new GameObject[4][];
		items[0] = new GameObject[1];
		items[1] = new GameObject[1];
		items[2] = new GameObject[2];
		items[3] = new GameObject[1];
	}

	void FixedUpdate()
	{
		if (!GameManager.tutorial && (PV.IsMine || !PhotonNetwork.InRoom))
        {
			Move();
			Manage();
			Use();
		}
		ManageItems();
	}

	public bool Move()
    {
		if (!PauseMenu.paused)
		{
			float movex = Input.GetAxis("Horizontal");
			float movey = Input.GetAxis("Vertical");

			if (Mathf.Abs(movex) + Mathf.Abs(movey) > 0)
			{
				Vector2 force = new Vector2(movex * maxSpeed, movey * maxSpeed);
				myRb.AddForce(force);

				if (movex > 0 && !facingRight || movex < 0 && facingRight)
				{
					transform.localScale = new Vector3(-(transform.localScale.x), transform.localScale.y, transform.localScale.z);
					facingRight = !facingRight;
				}
				return true;
			}
			else
			{
				Vector2 force = new Vector2(movex * maxSpeed, movey * maxSpeed);
				myRb.AddForce(force);
			}
		}
		return false;
	}

	public bool Use()
	{
		if (!PauseMenu.paused)
		{
			if (Input.GetMouseButton(0))
			{
				if (state == 0)
				{
					Catch();
				}
				else if (state == 1)
				{
					if (Time.time > nextFire && subMan.flareLoaded)
					{
						Sign();
						nextFire = Time.time + fireRate;
					}
					else if (Time.time > nextFire)
					{
						subMan.OutOfAmmo();
						nextFire = Time.time + fireRate;
					}
				}
				else if (state == 2)
				{
					if (Time.time > nextFire && (Camera.main.WorldToScreenPoint(target).y > centerY && subMan.topLoaded))
					{
						Shoot(true);
						nextFire = Time.time + fireRate;
						cameraShake.Shake();
					}
					else if (Time.time > nextFire && (Camera.main.WorldToScreenPoint(target).y <= centerY && subMan.botLoaded))
					{
						Shoot(false);
						nextFire = Time.time + fireRate;
						cameraShake.Shake();
					}
					else if (Time.time > nextFire)
					{
						subMan.OutOfAmmo();
						nextFire = Time.time + fireRate;
					}
				}
				return true;
			}
		}
		return false;
	}

	public bool Manage()
	{
		if (!PauseMenu.paused)
		{
			if (Hand()) return true;
			if (Signer()) return true;
			if (Cannons()) return true;
			if (Saver()) return true;
		}
		return false;
	}

	public bool Hand()
	{
		if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("sub_A_idle"))
		{
			if (state != 0)
			{
				subMan.ChangeState(0);

				state = 0;
				transition = false;

				hand = Instantiate(handPrefab, transform.position + Vector3.Scale(handPos, new Vector3(((facingRight) ? -1 : 1), 1, 1)), Quaternion.identity);
				hand.transform.parent = gameObject.transform;

				botArm = hand.transform.GetChild(0).gameObject;
				midArm1 = botArm.transform.GetChild(0).gameObject;
				midArm2 = midArm1.transform.GetChild(0).gameObject;
				claw = midArm2.transform.GetChild(0).gameObject;

				items[0][0] = hand;
			}
		}

		if (Input.GetKey(KeyCode.Alpha1))
		{
			myAnim.SetInteger("state", 0);
			return true;
		} 
		return false;
	}

	public bool Signer()
	{
		if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("sub_S_idle"))
		{
			if (state != 1)
			{
				subMan.ChangeState(1);

				state = 1;
				transition = false;

				signer = Instantiate(signerPrefab, transform.position + Vector3.Scale(signerPos, new Vector3(((facingRight) ? -1 : 1), 1, 1)), Quaternion.identity);
				signer.transform.parent = gameObject.transform;
				items[1][0] = signer;
			}
		}

		if (Input.GetKey(KeyCode.Alpha2))
		{
			myAnim.SetInteger("state", 1);
			return true;
		} 
		return false;
	}

	public bool Cannons()
	{
		if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("sub_D_idle"))
		{
			if (state != 2)
			{

				subMan.ChangeState(2);

				state = 2;
				transition = false;

				bottomCannon = Instantiate(bottomCannonPrefab, transform.position + Vector3.Scale(bottomCannonPos, new Vector3(((facingRight) ? -1 : 1), 1, 1)), new Quaternion(0, 0, 180, 0));
				bottomCannon.transform.parent = gameObject.transform;

				topCannon = Instantiate(topCannonPrefab, transform.position + Vector3.Scale(topCannonPos, new Vector3(((facingRight) ? -1 : 1), 1, 1)), Quaternion.identity);
				topCannon.transform.parent = gameObject.transform;

				items[2][0] = bottomCannon;
				items[2][1] = topCannon;
			}
		}

		if (Input.GetKey(KeyCode.Alpha3))
		{
			myAnim.SetInteger("state", 2);
			return true;
		} 
		return false;
	}

	public bool Saver()
	{
		if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("sub_F_idle"))
		{
			if (state != 3)
			{
				subMan.ChangeState(3);

				state = 3;
				transition = false;
			}
		}

		if (Input.GetKey(KeyCode.Alpha4))
		{
			myAnim.SetInteger("state", 3);
			return true;
		}
		return false;
	}

	public void ManageItems()
	{
		target = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));

		if (state == 3)
		{
			cross.transform.position = new Vector2(0, 200);
		}
		else
		{
			cross.transform.position = new Vector2(target.x, target.y);
		}

		if (IsTransitioning() && !transition)
		{
			transition = true;

			foreach (GameObject item in items[state])
			{
				Destroy(item);
			}
		}
		if (state == 0 && !transition)
		{
			ManageHand();
		}
	}

	bool IsTransitioning()
	{
		return !(myAnim.GetCurrentAnimatorStateInfo(0).IsName("sub_A_idle")
			|| myAnim.GetCurrentAnimatorStateInfo(0).IsName("sub_S_idle")
			|| myAnim.GetCurrentAnimatorStateInfo(0).IsName("sub_D_idle")
			|| myAnim.GetCurrentAnimatorStateInfo(0).IsName("sub_F_idle"));
	}

	public bool IsRetracted()
    {
		return retracted;
    }

	void RetractHand()
    {
		retracted = true;

		botArm.GetComponent<SpriteRenderer>().enabled = false;
		midArm1.GetComponent<SpriteRenderer>().enabled = false;
		midArm2.GetComponent<SpriteRenderer>().enabled = false;

		Debug.Log("Retracted");
    }

	public void ManageHand()
	{
		if (!Input.GetMouseButton(0))
		{
			if (arms == 1)
			{
				MechRetract(midArm1, 10f);
				MechRetract(midArm2, 10f);
				MechRetract(claw, 10f);
				MechDrop(botArm, 2f);
				if (botArm.transform.position.y - transform.position.y <= -1.2)
				{
					arms--;
					RetractHand();
				}
			}
			else if (arms == 2)
			{
				MechRetract(midArm1, 10f);
				MechRetract(midArm2, 10f);
				MechRetract(claw, 10f);
				MechDrop(botArm, 2f);
				if (botArm.transform.position.y - transform.position.y <= -0.8)
				{
					arms--;
					botArm.GetComponent<SpriteRenderer>().enabled = false;
					midArm1.GetComponent<SpriteRenderer>().enabled = false;
					midArm2.GetComponent<SpriteRenderer>().enabled = false;
				}
			}
			else if (arms == 3)
			{
				MechRetract(midArm1, 10f);
				MechRetract(midArm2, 10f);
				MechRetract(claw, 5f);
				MechDrop(botArm, 2f);
				if (botArm.transform.position.y - transform.position.y <= -0.4)
				{
					arms--;
					botArm.GetComponent<SpriteRenderer>().enabled = false;
					midArm1.GetComponent<SpriteRenderer>().enabled = false;
				}
			}
			else if (arms == 4)
			{
				MechRetract(midArm1, 10f);
				MechRetract(midArm2, 5f);
				MechRetract(claw, 2f);
				MechDrop(botArm, 2f);
				if (botArm.transform.position.y - transform.position.y <= 0.1)
				{
					arms--;
					botArm.GetComponent<SpriteRenderer>().enabled = false;
				}
			}
			else
			{
				MechRetract(midArm1, 10f);
				MechRetract(midArm2, 10f);
				MechRetract(claw, 10f);
				if (botArm.transform.position.y - transform.position.y > -1.2)
				{
					arms++;
				}
			}
		}
	}

	void MechRotate(GameObject arm, float speed)
	{
		Vector3 rot = new Vector3();
		rot.x = target.x - arm.transform.position.x;
		rot.y = target.y - arm.transform.position.y;

		float angle = (Mathf.Atan2(rot.y, rot.x) * Mathf.Rad2Deg) - 90;
		Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));

		arm.transform.rotation = Quaternion.Slerp(arm.transform.rotation, rotation, speed * Time.deltaTime);
	}

	void MechLift(GameObject arm, float speed)
	{
		arm.transform.position += Vector3.up * speed * Time.deltaTime;
	}

	void MechRetract(GameObject arm, float speed)
	{
		Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, 0));
		arm.transform.rotation = Quaternion.Slerp(arm.transform.rotation, rotation, speed * Time.deltaTime);
	}

	void MechDrop(GameObject arm, float speed)
	{
		arm.transform.position -= Vector3.up * speed * Time.deltaTime;
	}

	void Catch()
	{
		if (retracted)
        {
			retracted = false;
		}

		if (arms == 0)
		{
			MechLift(botArm, 1f);
			if (claw.transform.position.y - transform.position.y > 0.4)
			{
				arms++;
				midArm2.GetComponent<SpriteRenderer>().enabled = true;
			}
		}
		else if (arms == 1)
		{
			MechRotate(claw, 1f);
			MechLift(botArm, 1f);
			if (midArm2.transform.position.y - transform.position.y > 0.4)
			{
				arms++;
				midArm2.GetComponent<SpriteRenderer>().enabled = true;
				midArm1.GetComponent<SpriteRenderer>().enabled = true;
			}
		}
		else if (arms == 2)
		{
			MechRotate(claw, 1f);
			MechRotate(midArm1, 0.5f);
			MechLift(botArm, 1f);
			if (midArm1.transform.position.y - transform.position.y > 0.1)
			{
				arms++;
				midArm2.GetComponent<SpriteRenderer>().enabled = true;
				midArm1.GetComponent<SpriteRenderer>().enabled = true;
				botArm.GetComponent<SpriteRenderer>().enabled = true;
			}
		}
		else if (arms == 3)
		{
			MechRotate(claw, 1f);
			MechRotate(midArm1, 1f);
			MechRotate(midArm2, 0.5f);
			MechLift(botArm, 1f);
			if (botArm.transform.position.y - transform.position.y > 0.1)
			{
				arms++;
			}
		}
		else
		{
			MechRotate(claw, 1f);
			MechRotate(midArm1, 1f);
			MechRotate(midArm2, 1f);
		}			
	}

	public void SimulateShoot()
    {
		target = new Vector3(-3.64f, -8.55f);
		Shoot(true);
    }

	void Shoot(bool top) {

		Vector3 difference = target - transform.position;
		float rotationZ = Mathf.Atan2 (difference.y, difference.x) * Mathf.Rad2Deg;

		if (top)
        {
			shooter = transform.position + Vector3.Scale(topCannonPos, new Vector3(((facingRight) ? -1 : 1), 1, 1));
        } else
        {
			shooter = transform.position + Vector3.Scale(bottomCannonPos, new Vector3(((facingRight)? -1 : 1),1,1));
		}

		subMan.TorpedoSpend(top);

		GameObject bullet = PhotonNetwork.Instantiate (Path.Combine("prefabs", "items", "torpedo"), shooter, Quaternion.Euler (new Vector3 (0, 0, rotationZ)));
	}

	void Sign()
	{
		Vector3 difference = target - transform.position;
		float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

		shooter = transform.position + Vector3.Scale(topCannonPos, new Vector3(((facingRight)? -1 : 1),1,1));

		subMan.FlareSpend();

		GameObject sign = PhotonNetwork.Instantiate(Path.Combine("prefabs", "items", "sign_light"), shooter, Quaternion.Euler(new Vector3(0, 0, rotationZ)));
	}
}
