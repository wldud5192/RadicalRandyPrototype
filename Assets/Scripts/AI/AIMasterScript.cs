using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class AIMasterScript : MonoBehaviour
{
	public enum AIState
	{
		AI_Patroling,
		AI_Chasing,
		AI_Searching,
		AI_Alerted
	};

	public enum SearchType
	{
		Search_Snake,
		Search_Horizontal,
		Search_Vertical
	}

	float viewDistance;
	float walkSpeed;
	float runSpeed;
	public GameObject[] patrolPositions;
	public int nextPatrolPosition;
	bool canRun;
	bool isLethal;
	bool isChasingPlayer;
	NavMeshAgent navAgent;
	Vector3 playersLastLocation;
	float attackDistance;
	float searchTime;
	float searchDistance;
	public float minDistanceFromCheckpoint;

	public Vector3 alertPosition;

	public AI_SO aiLogic;
	public AIState currentState;
	public SearchType searchMethod;

	public bool moveUpOrRight;

	GameObject player;

	//public float viewDistance = 15;

	void Start()
	{
		GetOrCreateNavMesh();
		GetSOData();

		moveUpOrRight = true;
	}

	void Update()
	{

		switch (currentState)
		{
			case AIState.AI_Patroling:
				Search();

				//Patrol();
				break;

			case AIState.AI_Chasing:
				//Chase();
				Search();

				break;

			case AIState.AI_Searching:
				Search();

				break;

			case AIState.AI_Alerted:
				Search();

				//Alert(alertPosition);
				break;

			default:
				break;
		}
	}

	void Patrol()
	{
		if (Vector3.Distance(transform.position, patrolPositions[nextPatrolPosition].transform.position) < minDistanceFromCheckpoint) //Todo - Replace with SO Editable
		{
			if (patrolPositions.Length - 1 < nextPatrolPosition + 1)
			{
				nextPatrolPosition = 0;
			}
			else
			{
				nextPatrolPosition += 1;
			}
		}
		navAgent.SetDestination(patrolPositions[nextPatrolPosition].transform.position);
	}

	void Chase()
	{
		isChasingPlayer = true;

	}

	void Search()
	{
		Vector3 randomNavPosition = new Vector3(Random.Range(-50, 50), Random.Range(-50, 50));
		NavMeshHit hitPoint;
		NavMesh.SamplePosition(randomNavPosition, out hitPoint, 50, 1);

		//Scan Vector 3.Up * dist + -Vector3.forward * dist

		float distance = 0.0f;

		if (searchMethod == SearchType.Search_Horizontal)
		{
			RaycastHit hitRight;
			RaycastHit hitLeft;

			if (moveUpOrRight)
			{
				if (Physics.Raycast(this.transform.position, Vector3.right, out hitRight, 500))
				{
					distance = Vector3.Distance(transform.position, hitRight.transform.position);
					if (!ScanForPlayer(hitRight) && player == null)
					{
						navAgent.SetDestination(hitRight.transform.position);
						Debug.Log(distance);

						if (distance < 5)
						{
							moveUpOrRight = false;
						}
					}
				}
			}
			else
			{
				if (Physics.Raycast(this.transform.position, -Vector3.right, out hitLeft, 500))
				{
					if (!ScanForPlayer(hitLeft) && player == null)
					{
						distance = Vector3.Distance(transform.position, hitLeft.transform.position);

						navAgent.SetDestination(hitLeft.transform.position);

						if (distance < 5)
						{
							moveUpOrRight = true;
						}
					}
				}
			}
		}


		if (searchMethod == SearchType.Search_Vertical)
		{
			RaycastHit hitFront;
			RaycastHit hitBack;

			if (player == null)
			{
				if (moveUpOrRight)
				{
					if (Physics.Raycast(this.transform.position, Vector3.forward, out hitFront, 500))
					{
						distance = Vector3.Distance(transform.position, hitFront.transform.position);

						if (!ScanForPlayer(hitFront) && player == null)
						{
							navAgent.SetDestination(hitFront.transform.position);
							Debug.Log(distance);

							if (distance < 5)
							{
								moveUpOrRight = false;
							}
						}
					}
				}
				else
				{
					if (Physics.Raycast(this.transform.position, -Vector3.forward, out hitBack, 500))
					{
						distance = Vector3.Distance(transform.position, hitBack.transform.position);

						if (!ScanForPlayer(hitBack) && player == null)
						{
							navAgent.SetDestination(hitBack.transform.position);

							if (distance < 5)
							{
								moveUpOrRight = true;
							}
						}
					}
				}
			}
			else
			{
				navAgent.SetDestination(player.transform.position);

				if(Vector3.Distance(transform.position, player.transform.position) < attackDistance)
				{
					SceneManager.LoadScene(SceneManager.GetActiveScene().name);
				}
			}
		}

		Debug.DrawLine(transform.position, transform.position + Vector3.forward * 10, Color.red);
		Debug.DrawLine(transform.position, transform.position + -Vector3.forward * 10, Color.red);
		Debug.DrawLine(transform.position, transform.position + Vector3.right * 10, Color.red);
		Debug.DrawLine(transform.position, transform.position + -Vector3.right * 10, Color.red);
	}

	//Scans for player for set distance!
	bool ScanForPlayer(RaycastHit hit)
	{
		if (hit.transform.CompareTag("Player"))
		{
			if (Vector3.Distance(transform.position, hit.transform.position) < viewDistance)
			{
				player = hit.transform.gameObject;
				navAgent.SetDestination(player.transform.position);
				player.GetComponent<PlayerController>().playerIsDetected = true;
				return true;
			}
			return false;
		}
		return false;
	}

	void Alert(Vector3 alertPosition)
	{
		navAgent.SetDestination(alertPosition);
	}

	void GetOrCreateNavMesh()
	{
		navAgent = GetComponent<NavMeshAgent>();

		if (navAgent == null)
		{
			navAgent = gameObject.AddComponent<NavMeshAgent>();
		}
	}

	void GetSOData()
	{
		viewDistance = aiLogic.viewDistance;
		walkSpeed = aiLogic.walkSpeed;
		runSpeed = aiLogic.runSpeed;
		canRun = aiLogic.canRun;
		isLethal = aiLogic.isLethal;
		attackDistance = aiLogic.attackDistance;
		searchTime = aiLogic.searchTime;
		searchDistance = aiLogic.searchDistance;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.CompareTag("Player"))
		{
			isChasingPlayer = true;
			playersLastLocation = collision.transform.position;
			currentState = AIState.AI_Chasing;
		}

		if (isChasingPlayer = true)
		{
			StartCoroutine(SearchForPlayer());
		}
	}

	IEnumerator SearchForPlayer()
	{
		isChasingPlayer = false;
		currentState = AIState.AI_Searching;
		yield return new WaitForSeconds(searchTime);
		currentState = AIState.AI_Patroling;
	}

	//public float fovAngle = 110f;
	//public bool playerSighted = false;


	/*private void OnTriggerStay(Collider other)
	{
		float angle = Vector3.Angle(transform.position + Vector3.forward, transform.forward);

		if (angle < fovAngle * 0.5f)
		{
			RaycastHit hit;

			if (Physics.Raycast(transform.position + transform.up, Vector3.forward, out hit, fovDistance))
			{
				if (hit.collider.gameObject.CompareTag("Player"))
				{
					playerSighted = true;
					Debug.Log("I SEE YOU!");
				}
			}
		}


	}*/
}