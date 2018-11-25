using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour {
    
    float speed = 60f;
    AudioSource audio;
    GameObject player;
    GameObject door;

	// Use this for initialization
	void Start () {

        door = GameObject.FindGameObjectWithTag("Door");
        player = GameObject.FindGameObjectWithTag("Player");
        audio = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.up * speed * Time.deltaTime);
        }


    void OnTriggerEnter (Collider col)
    {
        if(col.gameObject == player)
        {
            StartCoroutine("MoveDoor");
            audio.Play();
            transform.position = Vector3.one * 9999f;
            Destroy(gameObject, audio.clip.length);
        }
    }

    IEnumerator MoveDoor ()
    {
        Destroy(door);
        yield return null;
        }

    }
