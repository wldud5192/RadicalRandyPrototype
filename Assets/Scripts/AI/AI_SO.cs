using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy SO",menuName = "Enemy SO")]
public class AI_SO : ScriptableObject
{
	public float viewDistance;
	public float walkSpeed;
	public float runSpeed;
	public GameObject[] patrolPositions;
	public bool canRun;
	public bool isLethal;
	public float attackDistance;
	public float searchTime;
	public float searchDistance;
}
