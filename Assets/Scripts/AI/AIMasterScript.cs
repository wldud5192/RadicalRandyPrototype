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

	float viewDistance;
	float walkSpeed;
	float runSpeed;
	GameObject[] patrolPositions;
	int nextPatrolPosition;
	bool canRun;
	bool isLethal;
	bool isChasingPlayer;
	NavMeshAgent navAgent;
	Vector3 playersLastLocation;
	float attackDistance;

	public Vector3 alertPosition;

	public AI_SO aiLogic;
	AIState currentState;

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
		if (patrolPositions.Length > 0 && patrolPositions != null)
		{
			if (Vector3.Distance(transform.position, patrolPositions[nextPatrolPosition].transform.position) < 10) //Todo - Replace with SO Editable
			{
				if (nextPatrolPosition > patrolPositions.Length)
				{
					nextPatrolPosition = 0;
				}
				else
				{
					nextPatrolPosition += 1;
				}
			}
		}
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
		patrolPositions = aiLogic.patrolPositions;
		canRun = aiLogic.canRun;
		isLethal = aiLogic.isLethal;
		attackDistance = aiLogic.attackDistance;
	}

	private void OnCollisionEnter(Collision collision)
	{
		
	}
}