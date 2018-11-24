using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterHackPickUp : MonoBehaviour
{

	public float speed;
	GameObject player;
	AudioSource audio;
	Renderer rend;
	public bool hiding = false;
	float cooldown;
	Material mat;
    float duration = 0.3f;

    void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		audio = GetComponent<AudioSource>();
		player = GameObject.Find("Character");
		rend = player.GetComponent<SkinnedMeshRenderer>();
	}

	void Update()
	{
        float lerp = Mathf.PingPong(Time.time, duration) / duration;
        transform.Rotate(Vector3.up * speed * Time.deltaTime);
        if (hiding)
        {
            //agent cannot detect player
            rend.material.color = Color.Lerp(Color.red, Color.green, lerp);
        }
	}


	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player" && !hiding)
		{
			hiding = true;
			cooldown = 10;
			audio.Play();
			transform.position = Vector3.one * 9999f;
		}
	}


}
