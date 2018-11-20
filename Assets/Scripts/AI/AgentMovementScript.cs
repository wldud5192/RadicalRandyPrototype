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
	GameObject player;

	[Header("AgentData")]
	public float Speed;
	public float dashSpeed;
	public float searchTime;
	public float distanceFromCheckpoint;
	public SearchState movementState;
	public float viewDistance;
	Vector3 moveDirection;

	public Transform navObjectParent;
	[SerializeField]
	List<Transform> navPositions;
	int currentTargetNode;

	[SerializeField]
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
		if (!chasingPlayer)
		{
			SearchingForPlayer();
		}

		if (!isIdle)
		{
			Wandering();
		}
		else
		{
			if (!isLookingAround)
			{
				StartCoroutine(IdleForTime());
			}
			transform.LookAt(navPositions[currentTargetNode]);
		}
	}

	void Wandering()
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

		if (navAgent.destination == null && !isLookingAround)
		{
			navAgent.SetDestination(navPositions[currentTargetNode].position);
			Debug.Log("Were heading to a new node");
		}
	}

	void SearchingForPlayer()
	{
		Debug.DrawLine(transform.position, transform.position + transform.forward * viewDistance);

		if (chasingPlayer == true)
		{
			return;
		}

		RaycastHit hit;
		if (Physics.Raycast(transform.position, transform.forward, out hit, viewDistance))
		{
			if (hit.transform != null && hit.transform.CompareTag("Player"))
			{
				player = hit.transform.gameObject;
				chasingPlayer = true;
			}
		}

	}

	void ChasingPlayer()
	{
		RaycastHit playerTarget;
		if (Physics.Raycast(transform.position, player.transform.position, out playerTarget, 1000.0f))
		{
			if (!playerTarget.transform.CompareTag("Player"))
			{
				chasingPlayer = false;
				Debug.Log("I've lost the player");
			}
			else
			{
				navAgent.SetDestination(player.transform.position);
			}
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
