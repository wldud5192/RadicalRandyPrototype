using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class AgentMovementScript : MonoBehaviour
{
	public enum SearchType
	{
		Search_Up,
		Search_Right
	}

	public enum SearchState
	{
		Patrol,
		Chasing,
		Searching
	}

	SearchType searchType;
	NavMeshAgent navAgent;
	bool movingUpRight;

	[Header("AgentData")]
	public float Speed;
	public float dashSpeed;
	public float searchTime;
	public float distanceFromCheckpoint;
	public SearchState movementState;
	float viewDistance;
	Vector3 moveDirection;

	public Transform navObjectParent;
	[SerializeField]
	List<Transform> navPositions;
	int currentTargetNode;

	bool chasingPlayer, isIdle, isLookingAround;

	float nodeDistance = 1.0f;

	private void Awake()
	{
		navAgent = GetComponent<NavMeshAgent>();
		Debug.Log(navObjectParent.name);
		navPositions = new List<Transform>();

		foreach (Transform childObj in navObjectParent.GetComponentsInChildren<Transform>())
		{
			if (childObj != navObjectParent)
			{
				navPositions.Add(childObj);
			}
		}
		navAgent.speed = dashSpeed;
		isIdle = true;
	}

	void Update()
	{
		if(!isIdle)
		{
			Search();
		}
		else
		{
			if(!isLookingAround)
			{
				StartCoroutine(IdleForTime());
			}
			transform.LookAt(navPositions[currentTargetNode]);
		}
	}

	void Search()
	{
		if (chasingPlayer)
		{
			Debug.Log("Were chasing the player");
			return;
		}

		if (Vector3.Distance(this.transform.position, navPositions[currentTargetNode].position) < nodeDistance)
		{
			if (navPositions.Count - 1 < currentTargetNode + 1)
			{
				currentTargetNode = 0;
			}
			else
			{
				currentTargetNode += 1;
			}

			isIdle = true;
			Debug.Log("Were close to the node. Lets look around");
		}

		if(navAgent.destination == null && !isLookingAround)
		{
			navAgent.SetDestination(navPositions[currentTargetNode].position);
			Debug.Log("Were heading to a new node");
		}
	}

	IEnumerator IdleForTime()
	{
		Debug.Log("Lets have a look around");
		isLookingAround = true;

		yield return new WaitForSeconds(searchTime);

		navAgent.SetDestination(navPositions[currentTargetNode].position);
		isIdle = false;
		isLookingAround = false;
		Debug.Log("Lets move on");
	}

}
