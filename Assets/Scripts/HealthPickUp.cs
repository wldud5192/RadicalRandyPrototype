using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class HealthPickUp : MonoBehaviour
{

    public float speed;
    AudioSource pickupSound;
    GameObject player;

    void Start()
    {
        pickupSound = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        transform.Rotate(Vector3.up * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject == player)
        {
            pickupSound.Play();
            player.GetComponent<PlayerHealth>().curLife += 1;
            transform.position = Vector3.one * 9999f;
            Destroy(gameObject, pickupSound.clip.length);
        }

    }
}
