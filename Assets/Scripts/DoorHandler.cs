using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandler : MonoBehaviour
{
	public Vector3 doorEndPosition;

	public Key_SO keyType;
	bool isUnlocked;

	void Update()
	{
		if(isUnlocked)
		{
			if(transform.position != doorEndPosition)
			{
				Vector3.MoveTowards(transform.position, doorEndPosition, 0.0f);
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.transform.CompareTag("Player"))
		{
			if(other.GetComponent<PlayerController>().keyList.Contains(keyType))
			{
				isUnlocked = true;
				other.GetComponent<PlayerController>().keyList.Remove(keyType);
			}
		}
	}
}
