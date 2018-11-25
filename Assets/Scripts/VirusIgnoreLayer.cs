using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusIgnoreLayer : MonoBehaviour
{
	Rigidbody virusRigidbody;
	public LayerMask layerToIgnore;

	void Start()
	{

		virusRigidbody = GetComponent<Rigidbody>();
		Physics.IgnoreLayerCollision(8, 9);
	}
}
