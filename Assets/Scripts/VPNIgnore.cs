using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VPNIgnore : MonoBehaviour
{
	List<AIMasterScript> enemiesList;

	private void Start()
	{
		foreach (AIMasterScript enemy in GameObject.FindObjectsOfType<AIMasterScript>())
		{
			enemiesList.Add(enemy);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			other.GetComponent<PlayerController>().invisibleToAI = true;
			Debug.Log("Player now invisible");
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			other.GetComponent<PlayerController>().invisibleToAI = false;
		}
	}
}
