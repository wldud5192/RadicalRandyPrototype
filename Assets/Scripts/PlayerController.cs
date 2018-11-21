using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{

	public float walkSpeed;
	public float runSpeed;
	Vector2 inputDirection;
	Vector3 startPosition;

	public bool playerIsDetected;
	Animator anim;
	Rigidbody playerRB;
	public float speed;
	public List<Key_SO> keyList;

	void Start()
	{
		keyList = new List<Key_SO>();
		anim = gameObject.GetComponent<Animator>();
		playerRB = GetComponent<Rigidbody>();

        transform.position += new Vector3(0,0.1f,0);

		walkSpeed = 2000;
		runSpeed = 2750;
        
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

		if(moveHorizontal == 0)
		{
			playerRB.velocity = new Vector3(0, 0, playerRB.velocity.z);
		}
		if(moveVertical == 0)
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
        }
		else
		{
			playerRB.AddForce(movement * walkSpeed * Time.deltaTime * 15);

            playerRB.velocity = Vector3.ClampMagnitude(playerRB.velocity, 1);
        }

	}

	void HandleInputRotation(float moveHorizontal, float moveVertical)
	{
		if (moveHorizontal > 0)
		{
			transform.rotation = Quaternion.Euler(0, 90,0);
		}
		else if (moveHorizontal < 0)
		{
			transform.rotation = Quaternion.Euler(0, 270,0);
		}
		else if(moveVertical > 0)
		{
			transform.rotation = Quaternion.Euler(0,0,0);
		}
		else if(moveVertical < 0)
		{
			transform.rotation = Quaternion.Euler(0,180,0);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.GetType() == typeof(Key_SO))
		{
			keyList.Add(other.GetComponent<Key_SO>());
			DestroyImmediate(other.gameObject);
		}
	}
}