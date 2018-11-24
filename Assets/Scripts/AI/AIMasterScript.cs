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

	float viewDistance;
	float fieldOfView = 110.0f;
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
	//public TextMesh seenTimer;

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
	}

	void Update()
	{
        seenBar.value = alertTime;
		//seenTimer.text = alertTime.ToString();

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

		Physics.OverlapSphereNonAlloc(transform.position + sphereOffset, searchBehindRange, sphereHitObjects);

		if (sphereHitObjects != null && sphereHitObjects.Length > 0)
		{
			foreach (Collider hitObject in sphereHitObjects)
			{
				if (hitObject != null)
				{
					if (hitObject.CompareTag("Player") && player == null)
					{
						StartPlayerDetection(hitObject.transform.gameObject);
					}
				}
			}
		}

		RaycastHit FoVHit;
		if (ScanInDirection(transform.forward, viewDistance, out FoVHit))
		{

			if (FoVHit.transform.gameObject != null)
			{
				if (FoVHit.transform.CompareTag("Player"))
				{
					StartPlayerDetection(FoVHit.transform.gameObject);
				}
			}

			Debug.DrawLine(transform.position, transform.position + transform.forward * viewDistance);
		}
		float distance = 0.0f;

		if (searchMethod == SearchType.Search_Horizontal)
		{
			RaycastHit hitRight;
			RaycastHit hitLeft;

			if (moveUpOrRight)
			{
				if (ScanInDirection(Vector3.right, 500, out hitRight))
				{
					distance = Vector3.Distance(transform.position, hitRight.transform.position);
					CheckNavAgent();
					navAgent.SetDestination(hitRight.transform.position);
					if (distance < wallHitDistance)
					{
						moveUpOrRight = false;
					}
				}
			}
			else
			{
				if (ScanInDirection(Vector3.left, 500, out hitLeft))
				{
					distance = Vector3.Distance(transform.position, hitLeft.transform.position);
					//Debug.Log(this.name);
					CheckNavAgent();
					navAgent.SetDestination(hitLeft.transform.position);
					if (distance < wallHitDistance)
					{
						moveUpOrRight = true;
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
				if (ScanInDirection(Vector3.forward, 500, out hitFront))
				{
					distance = Vector3.Distance(transform.position, hitFront.transform.position);
					CheckNavAgent();
					navAgent.SetDestination(hitFront.transform.position);
					if (distance < wallHitDistance)
					{
						moveUpOrRight = false;
					}
				}
			}
			else
			{
				if (ScanInDirection(Vector3.back, 500, out hitBack))
				{
					distance = Vector3.Distance(transform.position, hitBack.transform.position);
					CheckNavAgent();
					navAgent.SetDestination(hitBack.transform.position);
					if (distance < wallHitDistance)
					{
						moveUpOrRight = true;
					}
				}
			}
		}
	}

	void StartPlayerDetection(GameObject detectedPlayer)
	{
		//Debug.Log(alertTime);

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
		//playerAnim.SetBool("isDead", true);
		yield return new WaitForSeconds(1.5f);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		isHit = false;
	}

	void GetSOData()
	{
		viewDistance = aiLogic.viewDistance;
		walkSpeed = aiLogic.speed;
		attackDistance = aiLogic.attackDistance;
		searchTime = aiLogic.searchTime;
	}

	private void OnTriggerStay(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			Vector3 dir = other.transform.position - transform.position;
			float angle = Vector3.Angle(dir, transform.forward);

			if(angle < fieldOfView * 0.5f)
			{
				RaycastHit hit;
				if(Physics.Raycast(transform.position, other.transform.position, out hit))
				{
					if(hit.transform.CompareTag("Player"))
						StartPlayerDetection(other.gameObject);
				}
				Debug.DrawLine(transform.position, other.transform.position);
				//Debug.Log(angle);
			}
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