using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusMovement : MonoBehaviour {

    float speed;
    string direction;

	// Use this for initialization
	void Start () {
        speed = 80f;
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.up * speed * Time.deltaTime);
    }
}
