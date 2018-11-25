using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour {

    Vector3 basePos;
    Vector3 _closedPosition;
    float raiseHeight = -5f;
    float doorSpeed = 10f;
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
            Vector3 endpos = door.transform.position + new Vector3(0f, 0f, raiseHeight);
            StartCoroutine("MoveDoor", endpos);
            audio.Play();
            transform.position = Vector3.one * 9999f;
            Destroy(gameObject, audio.clip.length);
        }
    }

    IEnumerator MoveDoor (Vector3 endPos)
    {
        float t = 0f;
        Vector3 startPos = door.transform.position;
        while (t < 2f)
        {
            t += Time.deltaTime * 10f;
            door.transform.position = Vector3.Slerp(basePos, endPos, t);
            yield return null;
        }
    }
}
