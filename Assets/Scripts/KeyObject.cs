using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class KeyObject : MonoBehaviour
{
	public Key_SO keyType;

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			other.GetComponent<PlayerController>().keyList.Add(keyType);
			Destroy(this);
		}
	}
}