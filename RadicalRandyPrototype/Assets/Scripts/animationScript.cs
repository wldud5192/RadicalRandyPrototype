using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationScript : MonoBehaviour {

	Animator animator;

	// Use this for initialization
	void Start () {

		animator = gameObject.GetComponent<Animator> ();

	}
	
	// Update is called once per frame
	void Update () {

		if (Input.anyKey) {
			animator.SetBool ("isWalking", true);
		} else {
			animator.SetBool ("isWalking", false);
		}
	}
}
