using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(SphereCollider))]
public class AgentMovementScript : MonoBehaviour
{

	enum AI_State
	{
		Chasing,
		Searching
	}

	public enum MovementType
	{
		Horizontal,
		Vertical
	}

	public float viewDistance = 5.0f;
	public float fieldOfView = 110.0f;
	float walkSpeed;
	float runSpeed;
	NavMeshAgent navAgent;
	public float attackDistance;
	AI_State currentEnemyState;
	public MovementType movementType;
	public Slider SuspicionScale;
	float alertTimer;
	public float timeUntilAlerted;
	GameObject player;

	void OnValidate()
	{
		GetComponent<SphereCollider>().radius = viewDistance;
	}

	void Start()
	{
		currentEnemyState = AI_State.Searching;
		navAgent = GetComponent<NavMeshAgent>();
	}

	void Update()
	{
		if (player == null)
		{
			Debug.LogError("Failed to find player");
			return;
		}

		SuspicionScale.value = alertTimer;

		switch(currentEnemyState)
		{
			case AI_State.Chasing:
				Chase();
			break;

			case AI_State.Searching:
				Search();
			break;

			default:
				break;
		}
	}

	void Chase()
	{
		RaycastHit hitPlayer;
		if(Physics.Raycast(transform.position, player.transform.position, out hitPlayer, 1000))
		{
			if(hitPlayer.transform.CompareTag("Player") == false)
			{
				StopPlayerDetection();
			}
			else
			{
				navAgent.SetDestination(player.transform.position);
			}
		}

		if(Vector3.Distance(transform.position, player.transform.position) < attackDistance)
		{
			StartCoroutine(Death());
		}
	}

	void Search()
	{

	}

	void StartPlayerDetection()
	{ }

	void StopPlayerDetection()
	{ }

	IEnumerator Death()
	{
		yield return new WaitForSeconds(0.0f);
	}
}