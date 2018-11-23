using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterHackPickUp : MonoBehaviour
{

	public float speed;
	GameObject player;
	AudioSource audio;
	Shader shader1;
	Shader shader2;
	Renderer rend;
	public bool hiding = false;
	bool exiting;
	float cooldown;
	Material mat;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		audio = GetComponent<AudioSource>();
		player = GameObject.Find("Character");
		rend = player.GetComponent<SkinnedMeshRenderer>();
		shader1 = Shader.Find("XRay Shaders/Diffuse-XRay-Replaceable");
		shader2 = Shader.Find("DissolverShader/DissolveShader");
		mat = rend.material;
	}

	void Update()
	{
		transform.Rotate(Vector3.up * speed * Time.deltaTime);
		if (hiding)
		{
			mat.shader = shader2;
		}

		if (mat.shader != null)
			if (mat.shader == shader2)
				mat.SetFloat("_DissolveAmount", Mathf.Lerp(0, 60, Time.deltaTime));
	}


	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player" && !hiding/*&& !onCD*/)
		{
			hiding = true;
			cooldown = 10;
			audio.Play();
			transform.position = Vector3.one * 9999f;
			//onCD = true;
		}
	}


}
