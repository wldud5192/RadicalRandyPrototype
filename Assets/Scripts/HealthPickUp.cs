using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class HealthPickUp : MonoBehaviour
{

	public float speed;
	AudioSource pickupSound;
	HealthScript playerHP;
	void Start()
	{
		pickupSound = GetComponent<AudioSource>();
		playerHP = GameObject.FindObjectOfType<HealthScript>();
	}

	void Update()
	{
		transform.Rotate(Vector3.up * speed * Time.deltaTime);
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player")
		{
			pickupSound.Play();
			if (playerHP != null)
			{
				playerHP.curLife += 1;
			}
			transform.position = Vector3.one * 9999f;
			Destroy(gameObject, pickupSound.clip.length);
		}

	}
}
