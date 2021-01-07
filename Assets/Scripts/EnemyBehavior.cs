using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using Photon.Pun;

public class EnemyBehavior : MonoBehaviour {

	bool tutorial;

	Rigidbody2D rb;
	CircleCollider2D coll;
	Transform player;
	Vector3 target;

	float nextRandomTarget;
	Vector3 wanderCurTarget;
	bool wanderCurTargetChosen;
	float wanderAngle;

	bool chasing;
	float nextChase;
	float chaseInsist;

	[Range(1, 5)]
	public float maxSpeed;

	public float minDistance;
	private float range;
	private bool facingRight;

	Animator myAnim;

	float nextSound;
	public float soundRate;

	public AudioClip[] roars;
	public AudioClip hurtSound;

	AudioSource enemyAS;

	int layerMask;

	List<Vector3> path;
	List<Vector3> deviantPath;
	bool canChase;
	Vector3 chaseLastPos;
	bool returned;
	bool discovering;
	bool pathDefined;
	int pathCur;
	int deviantPathCur;
	float checkDistance;
	float chaseTrackTime;
	float mustCheckTime;
	int wanderFailed;

	int id;

	PlayerLog log;

	void Start()
	{
		if (PhotonNetwork.InRoom && !GameManager.pilot)
		{
			this.enabled = false;
		}
		
		rb = GetComponent<Rigidbody2D>();

		coll = transform.Find("CollSurface").GetComponent<CircleCollider2D>();
		player = GameObject.FindWithTag("Player").transform;

		log = GameObject.Find("PlayerLog").GetComponent<PlayerLog>();

		myAnim = GetComponent<Animator>();

		canChase = true;
		returned = true;
		facingRight = true;

		enemyAS = GetComponent<AudioSource>();

		layerMask = 1 << LayerMask.NameToLayer("Cave");

		wanderAngle = Random.Range(0, 360);

		path = new List<Vector3>();
		path.Add(transform.position);

		deviantPath = new List<Vector3>();
	}

	void FixedUpdate()
	{
		range = Vector2.Distance(transform.position, player.position);
		myAnim.SetFloat("distance", Mathf.Abs(range));

		RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), new Vector2(player.position.x - transform.position.x, player.position.y - transform.position.y).normalized, range, 1 << LayerMask.NameToLayer("Cave"));

		UnityEngine.Debug.DrawRay(new Vector2(transform.position.x, transform.position.y), new Vector2(player.position.x - transform.position.x, player.position.y - transform.position.y).normalized * range, Color.red);

		if (range < minDistance && canChase && (chasing || (hit.collider == null))) // || hit.collider.gameObject.layer != LayerMask.NameToLayer("Cave") //&& (hit.collider == null)
		{
			if (!chasing)
			{
				if (!GameManager.tutorial)
				{
					log.StartChase(id, transform.position, player.position);
				}
				chasing = true;
				chaseLastPos = transform.position;
				chaseInsist = 100f;
				NewTarget(player.position);
				deviantPath.Add(transform.position);
				deviantPathCur = 0;
			}
			ChasePlayer();

			if (hit.collider != null)
            {
				chaseInsist -= range / 100;
			}

			if (range < minDistance / 2)
			{
				Roar();
				Camera.main.GetComponentInChildren<Tense>().TenseAmbient();
			}
			if (chaseInsist < 0 || deviantPath.Count > 50)
			{
				canChase = false;
			}
		}
		else if (!GameManager.tutorial)
		{
			if (chasing)
			{
				log.StopChase(id);

				chasing = false;
				returned = false;
				deviantPath.Reverse();
				deviantPathCur = 0;
			}
			if (returned)
			{
				if (pathDefined)
				{
					FollowPath();
				}
				else
				{
					Wander();
				}
			}
			else
			{
				Return();
			}
		}
	}

	public void SetId(int newId)
    {
		id = newId;
    }

	public int GetId()
	{
		return id;
	}

	void Follow(Vector3 pos, float speed)
    {
		rb.AddForce((pos - transform.position).normalized * speed);
		Flip();
	}

	void ChasePlayer()
	{
		target = player.position;

		chaseInsist -= range/150;

		Follow(player.position, Mathf.Min((maxSpeed / (range/1.7f)), maxSpeed));

		if (Vector2.Distance(chaseLastPos, transform.position) > 2f)
        {
			Track();
        }

		//if (!Moving(1f))
		//{
		//	canChase = false;
		//}
	}
	
	void Return()
    {
		if (Vector2.Distance(deviantPath[deviantPathCur], transform.position) < 1f)
        {
			deviantPathCur++;
			if (deviantPathCur >= deviantPath.Count)
            {
				returned = true;
				canChase = true;
				for (int i = 0; i < deviantPath.Count - 1; i++)
				{
					UnityEngine.Debug.DrawLine(deviantPath[i], deviantPath[i + 1], Color.red, 2000f);
				}
				deviantPath.Clear();
            } 
			else
            {
				NewTarget(deviantPath[deviantPathCur]);

			}
        }
		if (!returned)
		{
			Follow(deviantPath[deviantPathCur], 1f);
		}
	}

	void Track()
    {
		chaseLastPos = transform.position;
		deviantPath.Add(chaseLastPos);
	}

	void FollowPath()
    {
		if (Vector2.Distance(transform.position, path[pathCur]) < 1f)
		{
			pathCur++;
			if (pathCur >= path.Count)
            {
				path.Reverse();
				pathCur = 1;
            }
			NewTarget(path[pathCur]);
		}
		Follow(path[pathCur], 1f);

		if (!Moving())
		{
			pathDefined = false;
			path.Clear();
			path.Add(transform.position);
			pathCur = 0;

			wanderCurTarget = path[pathCur];
			NewTarget(wanderCurTarget);

			wanderCurTargetChosen = true;
		}
	}

	void Wander()
	{
		if (!wanderCurTargetChosen)
		{
			discovering = true;
			wanderAngle = Random.Range(0, 360);
			Vector3 temp = transform.position + Quaternion.AngleAxis(wanderAngle, Vector3.forward) * Vector3.right * 4f;

			if (pathCur == 0 || (pathCur == 1 && Vector2.Distance(temp, path[pathCur - 1]) > 4f) || (Vector2.Distance(temp, path[pathCur - 1]) > 4f && Vector2.Distance(temp, path[pathCur - 2]) > 4f))
			{
				wanderCurTarget = temp;
				NewTarget(wanderCurTarget);
				wanderCurTargetChosen = true;
				checkDistance = 1000f;
				wanderFailed = 0;
			} 
			else
            {
				wanderFailed++;

				if (wanderFailed > 20)
                {
					path.Clear();
					path.Add(transform.position);
					pathCur = 0;

					wanderCurTarget = path[pathCur];
					NewTarget(wanderCurTarget);

					wanderCurTargetChosen = true;

					wanderFailed = 0;
				}
            }
		}
		else
		{
			Follow(wanderCurTarget, 1.5f);

			if (Vector2.Distance(transform.position, wanderCurTarget) < 1f)
			{
				if (discovering)
				{
					pathCur++;
					path.Add(wanderCurTarget);

					if (path.Count > 30)
					{
						pathDefined = true;

						for (int i = 0; i < path.Count - 1; i++)
						{
							UnityEngine.Debug.DrawLine(path[i], path[i + 1], Color.green, 2000f);
						}
					}
					else
					{
						wanderCurTargetChosen = false;
						UnityEngine.Debug.DrawLine(path[pathCur - 1], path[pathCur], Color.white, 20f);
					}
				}
				else
                {
					wanderCurTargetChosen = false;
				}				
			} 
			else
            {
				if (!Moving(1f))
                {
					if (discovering)
					{
						wanderCurTarget = path[path.Count - 1];
						NewTarget(wanderCurTarget);

						wanderCurTargetChosen = true;
						discovering = false;
					}
                    else
                    {
                        path.Clear();
                        path.Add(transform.position);
                        pathCur = 0;

                        wanderCurTarget = path[pathCur];
                        NewTarget(wanderCurTarget);

                        wanderCurTargetChosen = true;
                    }
                }
            }
		}
	}

	void Flip()
	{
		if (transform.position.x > target.x && facingRight || transform.position.x < target.x && !facingRight)
		{
			Vector2 SpriteScale = transform.localScale;
			SpriteScale.x = -(SpriteScale.x);
			transform.localScale = SpriteScale;
			facingRight = !facingRight;
		}
	}

	void NewTarget(Vector3 newTarget)
    {
		target = newTarget;
		checkDistance = Vector2.Distance(transform.position, target);
		mustCheckTime = Time.time + 1f;
	}

	bool Moving(float margin = 0)
    {
		if (Time.time > mustCheckTime)
		{
			float distance = Vector2.Distance(transform.position, target);

			if (checkDistance - distance > margin)
			{
				checkDistance = distance;
				mustCheckTime = Time.time + 1f;
			} 
			else
            {
				return false; // stopped
            }
		}
		return true;
	}

	public void Roar()
	{
		if (!enemyAS.isPlaying && Time.time > nextSound)
		{
			enemyAS.clip = roars[Random.Range(0, roars.Length)];

			enemyAS.Play();
			nextSound = Time.time + soundRate;
		}
	}

	public void Hurt()
	{
		if (!GameManager.tutorial)
		{
			chasing = true;
			canChase = false;
		}

		enemyAS.clip = hurtSound;
		enemyAS.Play();
		nextSound = Time.time + soundRate;
	}
}