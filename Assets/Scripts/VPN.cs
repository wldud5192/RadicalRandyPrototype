using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VPN : MonoBehaviour
{
	AIMasterScript[] levelAI;

	void Start()
	{
		levelAI = GameObject.FindObjectsOfType<AIMasterScript>();
	}

	/*private void OnTriggerEnter(Collider other)
	{
		foreach (AIMasterScript AI in levelAI)
		{
			if (AI.player != null)
			{
				AI.player = null;
				AI.ReturnToStartPosition();
			}
		}
	}*/
}
