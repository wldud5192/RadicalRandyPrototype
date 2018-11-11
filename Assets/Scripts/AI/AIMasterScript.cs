using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class AIMasterScript : MonoBehaviour
{
	enum AIState
	{
		AI_Patroling,
		AI_Chasing,
		AI_Searching,
		AI_Alerted
	};

	enum SearchType
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
	AIState currentState;
	SearchType searchMethod;

	void Start()
	{
		GetOrCreateNavMesh();
		GetSOData();
	}

	void Update()
	{
		switch (currentState)
		{
			case AIState.AI_Patroling:
				Patrol();
			break;

			case AIState.AI_Chasing:
				Chase();
			break;

			case AIState.AI_Searching:
				Search();
			break;

			case AIState.AI_Alerted:
				Alert(alertPosition);
			break;

			default:
				break;
		}
	}

	void Patrol()
	{
		if (Vector3.Distance(transform.position, patrolPositions[nextPatrolPosition].transform.position) < minDistanceFromCheckpoint) //Todo - Replace with SO Editable
		{
			if(patrolPositions.Length -1 < nextPatrolPosition + 1)
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
		Vector3 randomNavPosition = new Vector3(Random.Range(-50,50), Random.Range(-50,50));
		NavMeshHit hitPoint;
		NavMesh.SamplePosition(randomNavPosition, out hitPoint, 50, 1);
	}

	void Alert(Vector3 alertPosition)
	{
		navAgent.SetDestination(alertPosition);
	}

	void GetOrCreateNavMesh()
	{
		navAgent = GetComponent<NavMeshAgent>();

		if(navAgent == null)
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
		if(collision.transform.CompareTag("Player"))
		{
			isChasingPlayer = true;
			playersLastLocation = collision.transform.position;
			currentState = AIState.AI_Chasing;
		}
		
		if(isChasingPlayer = true)
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
}