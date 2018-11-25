using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class AIMasterScript : MonoBehaviour
{
    
	public enum AIState
	{
		AI_Chasing,
		AI_Searching,
	};

	public enum SearchType
	{
		Search_Horizontal,
		Search_Vertical
	}

	public float viewDistance;
	public float fieldOfView = 110.0f;
	float walkSpeed;
	float runSpeed;
	public GameObject[] patrolPositions;
	public int nextPatrolPosition;
	bool canRun;
	bool isLethal;
	NavMeshAgent navAgent;
	Vector3 playersLastLocation;
	float attackDistance;
	float searchTime;
	float searchDistance;
	public float minDistanceFromCheckpoint;

	public Vector3 alertPosition;

	bool isHit = false;
	PlayerController playerCont;
	Animator playerAnim;
	AudioSource playerHurt;
	HealthScript playerHealthUI;
	public AI_SO aiLogic;
	public AIState currentState;
	public SearchType searchMethod;

	public bool moveUpOrRight;
	public float wallHitDistance;

	[HideInInspector]
	public GameObject player;

	public float searchBehindRange;
	Collider[] sphereHitObjects;
	public Vector3 sphereOffset;
	float AIRunSpeed;

	public float timeUntilAlerted;
	private float alertTime;

    public Slider seenBar;

	Vector3 initialPosition;

	private void OnValidate()
	{
		GetComponent<SphereCollider>().radius = viewDistance;
		foreach (Collider collision in GetComponents<Collider>())
		{
			collision.isTrigger = true;
		}
	}

	void Start()
	{
		GameObject player = GameObject.FindObjectOfType<PlayerController>().gameObject;
		playerCont = player.GetComponent<PlayerController>();
		playerAnim = player.GetComponent<Animator>();
		playerHurt = GetComponent<AudioSource>();

		playerHealthUI = GameObject.FindObjectOfType<HealthScript>();
		GetSOData();

		moveUpOrRight = true;

		if (wallHitDistance <= 0)
		{
			wallHitDistance = 5;
		}

		currentState = AIState.AI_Searching;
		navAgent = GetComponent<NavMeshAgent>();
		initialPosition = transform.position;
	}

	void Update()
	{
        seenBar.value = alertTime;

		switch (currentState)
		{
			case AIState.AI_Chasing:
				Chase();
				break;

			case AIState.AI_Searching:
				Search();
				break;

			default:
				break;
		}
	}

	void Chase()
	{
		if (player == null)
		{
			currentState = AIState.AI_Searching;
		}

        playerCont.playerIsDetected = true;
        navAgent.SetDestination(player.transform.position);

		RaycastHit playerScanHit;
		if(Physics.Raycast(transform.position, player.transform.position, out playerScanHit, 1000))
		{
			if(!playerScanHit.transform.CompareTag("Player"))
			{
				StopDetection();
			}
		}

		if (Vector3.Distance(transform.position, player.transform.position) < attackDistance)
		{
			if (!isHit)
			{
				StartCoroutine(Death());
			}
		}
	}

	void Search()
	{
		if (player != null)
		{
			currentState = AIState.AI_Chasing;
		}

		/*RaycastHit FoVHit;
		if (ScanInDirection(transform.forward, viewDistance, out FoVHit, false))
		{
			if (FoVHit.transform.gameObject != null)
			{
				if (FoVHit.transform.CompareTag("Player"))
				{
					StartPlayerDetection(FoVHit.transform.gameObject);
				}
				StopDetection();
			}
		}*/

		float distance = 0.0f;

		if (searchMethod == SearchType.Search_Horizontal)
		{
			RaycastHit hitRight;
			RaycastHit hitLeft;

			if (moveUpOrRight)
			{
				if (ScanInDirection(Vector3.right, 500, out hitRight, true))
				{
					distance = Vector3.Distance(transform.position, hitRight.transform.position);
					CheckNavAgent();

					if (navAgent.isOnNavMesh)
					{
						navAgent.SetDestination(hitRight.transform.position);
						if (distance < wallHitDistance)
						{
							moveUpOrRight = false;
						}
					}
				}
			}
			else
			{
				if (ScanInDirection(Vector3.left, 500, out hitLeft, true))
				{
					distance = Vector3.Distance(transform.position, hitLeft.transform.position);
					//Debug.Log(this.name);
					CheckNavAgent();
					if (navAgent.isOnNavMesh)
					{
						navAgent.SetDestination(hitLeft.transform.position);
						if (distance < wallHitDistance)
						{
							moveUpOrRight = true;
						}
					}
				}
			}
		}

		if (searchMethod == SearchType.Search_Vertical && player == null)
		{
			RaycastHit hitFront;
			RaycastHit hitBack;

			if (moveUpOrRight)
			{
				if (ScanInDirection(Vector3.forward, 500, out hitFront, true))
				{
					distance = Vector3.Distance(transform.position, hitFront.transform.position);
					CheckNavAgent();
					if (navAgent.isOnNavMesh)
					{
						navAgent.SetDestination(hitFront.transform.position);

						if (distance < wallHitDistance)
						{
							moveUpOrRight = false;
						}
					}
				}
			}
			else
			{
				if (ScanInDirection(Vector3.back, 500, out hitBack, true))
				{
					distance = Vector3.Distance(transform.position, hitBack.transform.position);
					CheckNavAgent();
					if (navAgent.isOnNavMesh)
					{
						navAgent.SetDestination(hitBack.transform.position);
						if (distance < wallHitDistance)
						{
							moveUpOrRight = true;
						}
					}
				}
			}
		}
	}

	void StartPlayerDetection(GameObject detectedPlayer)
	{
		if(Vector3.Distance(transform.position, detectedPlayer.transform.position) < 2)
		{
			alertTime = timeUntilAlerted;
		}

		if (alertTime <= timeUntilAlerted)
		{
			alertTime += Time.deltaTime;
		}

		if (alertTime >= timeUntilAlerted)
		{
			player = detectedPlayer;
		}
	}

	void StopDetection()
	{
		if (alertTime > 0)
		{
			alertTime -= Time.deltaTime / 2.5f;
			navAgent.isStopped = false;
		}
		
		if(alertTime <= 0)
		{
			player = null;
			currentState = AIState.AI_Searching;
		}
	}

	bool ScanInDirection(Vector3 direction, float distance, out RaycastHit hitObj, bool ignorePlayer = false)
	{
		RaycastHit hit = new RaycastHit();
		hitObj = new RaycastHit();

		if (Physics.Raycast(this.transform.position, direction, out hit, distance))
		{
			distance = Vector3.Distance(transform.position, hit.transform.position);

			if (!ignorePlayer)
			{
				if (hit.transform.CompareTag("Player"))
				{
					navAgent.Stop();
					Debug.Log("Seen player");
				}
				else
				{
					if(navAgent.isStopped)
					{
						navAgent.isStopped = false;
						Debug.Log("Cant see player");
					}
				}
			}

			hitObj = hit;
			return true;
		}
		return false;
	}

	void Alert(Vector3 alertPosition)
	{
		navAgent.SetDestination(alertPosition);
	}

	IEnumerator Death()
	{
        Destroy(player.GetComponent<BoxCollider>());
		playerCont.enabled = false;
		playerAnim.SetBool("isDead", true);

		if (isHit == false)
		{
			if (playerHealthUI != null)
			{
				playerHealthUI.curLife -= 1;
			}
			playerHurt.Play();
			isHit = true;
		}

		yield return new WaitForSeconds(1.5f);

		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		isHit = false;
	}

	void GetSOData()
	{
		//viewDistance = aiLogic.viewDistance;
		walkSpeed = aiLogic.speed;
		attackDistance = aiLogic.attackDistance;
		searchTime = aiLogic.searchTime;
	}


	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			Vector3 dir = other.transform.position - transform.position;
			Debug.Log("Dir " + dir);
			float angle = Vector3.Angle(dir, transform.forward);
			Debug.Log("Angle: " + angle);

			Debug.DrawLine(transform.position, other.transform.position, Color.red);

			if (angle < fieldOfView * 0.5f)
			{
				Debug.DrawLine(transform.position, other.transform.position, Color.green);
				StartPlayerDetection(other.gameObject);
				navAgent.isStopped = true;
				navAgent.speed = 0;
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			StopDetection();
			navAgent.isStopped = false;
			navAgent.speed = 10.0f;
		}
	}

	void CheckNavAgent()
	{
		if(navAgent == null)
		{
			this.GetComponent<NavMeshAgent>();
		}
	}
}