using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
	public AudioClip normalBGM;
	public AudioClip alertedBGM;
	AudioSource bgm;
	public float walkSpeed;
	public float runSpeed;
	Vector2 inputDirection;
	Vector3 startPosition;
	public bool played = false;

	GameObject canvas;

	GameObject alertedUI;
	public bool playerIsDetected;
	Animator anim;
	[HideInInspector]
	public bool invisibleToAI;

	Rigidbody playerRB;

	void Start()
	{
		canvas = GameObject.Find("Canvas");

		if (canvas != null)
		{
			bgm = canvas.GetComponent<AudioSource>();
			alertedUI = canvas.transform.Find("AlertedUI").gameObject;
			ChangeBGM();
		}


		anim = gameObject.GetComponent<Animator>();
		playerRB = GetComponent<Rigidbody>();

		transform.position += new Vector3(0, 0.1f, 0);

		walkSpeed = 1000;
		runSpeed = 1500;

		playerRB.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
		playerRB.useGravity = true;
	}


	void Update()
	{
		float moveHorizontal = Input.GetAxisRaw("Horizontal");
		float moveVertical = Input.GetAxisRaw("Vertical");

		Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
		movement = movement.normalized;

		HandleInputRotation(moveHorizontal, moveVertical);

		if (moveHorizontal == 0)
		{
			playerRB.velocity = new Vector3(0, 0, playerRB.velocity.z);
		}
		if (moveVertical == 0)
		{
			playerRB.velocity = new Vector3(playerRB.velocity.x, 0, 0);
		}

		if (movement.magnitude != 0)
		{
			if (playerIsDetected)
			{
				anim.SetBool("isRunning", true);
			}
			else
			{
				anim.SetBool("isWalking", true);
			}
		}
		else
		{
			if (playerIsDetected)
			{
				anim.SetBool("isRunning", false);
			}
			else
			{
				anim.SetBool("isWalking", false);
			}
		}

		if (playerIsDetected)
		{
			playerRB.AddForce(movement * runSpeed * Time.deltaTime * 15);
			playerRB.velocity = Vector3.ClampMagnitude(playerRB.velocity, 3);
			if (!played)
			{
				if (bgm != null && alertedUI != null)
				{
					ChangeBGM();
					alertedUI.SetActive(true);
				}
				played = true;
			}

		}
		else
		{
			playerRB.AddForce(movement * walkSpeed * Time.deltaTime * 15);
			playerRB.velocity = Vector3.ClampMagnitude(playerRB.velocity, 1);
			if (played)
			{
				if (bgm != null && alertedUI != null)
				{
					ChangeBGM();
					alertedUI.SetActive(false);
				}
				played = false;
			}
		}

	}

	void ChangeBGM()
	{
		if (bgm != null)
		{
			if (bgm.clip == normalBGM)
			{
				bgm.Stop();
				bgm.clip = alertedBGM;
				bgm.Play();
			}
			else
			{
				bgm.Stop();
				bgm.clip = normalBGM;
				bgm.Play();
			}
		}
	}

	void HandleInputRotation(float moveHorizontal, float moveVertical)
	{
		if (moveHorizontal > 0)
		{
			transform.rotation = Quaternion.Euler(0, 90, 0);
		}
		else if (moveHorizontal < 0)
		{
			transform.rotation = Quaternion.Euler(0, 270, 0);
		}
		else if (moveVertical > 0)
		{
			transform.rotation = Quaternion.Euler(0, 0, 0);
		}
		else if (moveVertical < 0)
		{
			transform.rotation = Quaternion.Euler(0, 180, 0);
		}
	}

}