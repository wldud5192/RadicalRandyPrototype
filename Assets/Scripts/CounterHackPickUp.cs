using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterHackPickUp : MonoBehaviour {

    //Reference to All AIs
    public float speed;
    AudioSource pickupSound;
    GameObject player;

    void Start ()
    {
        pickupSound = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        transform.Rotate(Vector3.up * speed * Time.deltaTime);
    }

    void OnTriggerEnter (Collider col)
    {
        if (col.gameObject == player)
        {
            pickupSound.Play();
            //AI.CanSeePlayer is disabled
            transform.position = Vector3.one * 9999f;
            Destroy(gameObject, pickupSound.clip.length);
        }

    }
}
