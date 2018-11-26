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
	public float wallHitDistance = 0.1f;

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

	GameObject tempPlayer;

	bool hitPlayer;

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

		tempPlayer = GameObject.FindObjectOfType<PlayerController>().gameObject;

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
		wallHitDistance = 0;
		navAgent.SetDestination(player.transform.position);

		if (player == null)
		{
			currentState = AIState.AI_Searching;
		}

		playerCont.playerIsDetected = true;

		Vector3 playerDirection = tempPlayer.transform.position - transform.position;
		playerDirection.y = 0;
		playerDirection.Normalize();

		Debug.DrawRay(transform.position, playerDirection * 100);

		RaycastHit playerScanHit;

		if (Physics.Raycast(transform.position, playerDirection, out playerScanHit, 1000))
		{
			if (!playerScanHit.transform.CompareTag("Player"))
			{
				StopPlayerDetection();
			}
		}


		if (player != null && Vector3.Distance(transform.position, player.transform.position) < attackDistance)
		{
			if (!isHit)
			{
				StartCoroutine(Death());
			}
		}

		//Debug.Log("Chasing");
	}

	void Search()
	{
		//Debug.Log("Searching");
		if (player != null)
		{
			currentState = AIState.AI_Chasing;
		}

		if (alertTime == timeUntilAlerted)
		{
			navAgent.SetDestination(tempPlayer.transform.position);
			currentState = AIState.AI_Chasing;
		}

		Vector3 playerDirection = tempPlayer.transform.position - transform.position;
		playerDirection.y = 0;
		playerDirection.Normalize();

		Debug.DrawRay(transform.position, playerDirection * 100);

		RaycastHit hitPlyr;
		if (Physics.Raycast(this.transform.position, playerDirection, out hitPlyr, 5000))
		{
			//Debug.Log(hitPlyr.transform.name);
			Debug.DrawLine(this.transform.position, hitPlyr.point, Color.blue);

			if (hitPlyr.transform.CompareTag("Player"))
			{
				hitPlayer = true;
			}
		}

		playerCont.playerIsDetected = false;
		StopPlayerDetection();

		float distance = 0.0f;

		if (searchMethod == SearchType.Search_Horizontal)
		{
			//Debug.Log("Searching Horizontal");
			RaycastHit hitRight;
			RaycastHit hitLeft;

			if (moveUpOrRight)
			{
				if (ScanInDirection(Vector3.right, 500, out hitRight))
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
				if (ScanInDirection(Vector3.left, 500, out hitLeft))
				{
					distance = Vector3.Distance(transform.position, hitLeft.transform.position);
					////Debug.Log(this.name);
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

			//Debug.Log("Searching Vertical");

			if (moveUpOrRight)
			{
				if (ScanInDirection(Vector3.forward, 500, out hitFront))
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
				if (ScanInDirection(Vector3.back, 500, out hitBack))
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
		if (alertTime > 0.1 || alertTime < timeUntilAlerted - 0.1)
		{
			navAgent.isStopped = true;
		}
		else
		{
			navAgent.isStopped = false;
		}

		if (Vector3.Distance(transform.position, detectedPlayer.transform.position) < 2)
		{
			alertTime = timeUntilAlerted;
			currentState = AIState.AI_Chasing;
		}


		if (alertTime <= timeUntilAlerted)
		{
			alertTime += Time.deltaTime;
			//navAgent.isStopped = true;
		}

		if (alertTime >= timeUntilAlerted)
		{
			player = detectedPlayer;
			currentState = AIState.AI_Chasing;
		}
	}

	void StopPlayerDetection()
	{
		if (alertTime > 0)
		{
			alertTime -= Time.deltaTime / 2.5f;
			//navAgent.isStopped = false;
		}

		if (alertTime <= 0.1)
		{
			currentState = AIState.AI_Searching;
			playerCont.playerIsDetected = false;
			player = null;
		}
	}

	bool ScanInDirection(Vector3 direction, float distance, out RaycastHit hitObj)
	{
		RaycastHit hit = new RaycastHit();
		hitObj = new RaycastHit();

		if (Physics.Raycast(this.transform.position, direction, out hit, distance))
		{
			distance = Vector3.Distance(transform.position, hit.transform.position);

			hitObj = hit;
			return true;
		}
		return false;
	}

	/*
	void ScanForPlayer()
	{
		if (tempPlayer == null)
		{
			tempPlayer = GameObject.FindObjectOfType<PlayerController>().gameObject;
		}

		RaycastHit playerHitCast;

		if (Physics.Raycast(transform.position, tempPlayer.transform.position, out playerHitCast))
		{
			Debug.DrawLine(transform.position, playerHitCast.transform.position, Color.yellow);
			//Debug.Log(playerHitCast.transform.name);

			if (playerHitCast.transform.CompareTag("Player"))
			{
				hitPlayer = true;
			}
		}
	}
	*/

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
			if (other.GetComponent<PlayerController>().invisibleToAI != true)
			{
				if (hitPlayer == true)
				{
					Vector3 dir = other.transform.position - transform.position;
					float angle = Vector3.Angle(dir, transform.forward);
					if (angle < fieldOfView * 0.5f)
					{
						StartPlayerDetection(other.gameObject);
					}
				}
			}
			else
			{
				StopPlayerDetection();
				navAgent.isStopped = false;
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			StopPlayerDetection();
			navAgent.isStopped = false;
			navAgent.speed = 10.0f;
		}
	}

	void CheckNavAgent()
	{
		if (navAgent == null)
		{
			this.GetComponent<NavMeshAgent>();
		}
	}
}