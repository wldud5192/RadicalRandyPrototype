using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AntiVirusScript : MonoBehaviour
{

	public TMP_Text removingText;
	GameObject[] virus;
	AudioSource audio;
	ParticleSystem pe;
	bool activated;
	float t = 0f;
	float duration = 5.5f;

	// Use this for initialization
	void Start()
	{
		removingText = GameObject.Find("AntiVirusActivationUI").GetComponent<TMP_Text>();
		audio = GetComponent<AudioSource>();
		pe = GetComponent<ParticleSystem>();

		if (GameObject.FindGameObjectsWithTag("Virus") != null)
		{
			virus = GameObject.FindGameObjectsWithTag("Virus");
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (activated)
		{
			removingText.color = Color.Lerp(Color.green, Color.black, t);
			if (t < 1)
			{
				t += Time.deltaTime / duration;
			}
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player" && !activated)
		{
			activated = true;
			audio.Play();
			pe.enableEmission = false;
			//Play sound
			for (int i = 0; i < virus.Length; i++)
			{
				Destroy(virus[i], 2.5f);
			}
		}

	}
}
