using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy SO",menuName = "Enemy SO")]
public class AI_SO : ScriptableObject
{
	public float viewDistance;
	public float speed;
	public float attackDistance;
	public float searchTime;
}
