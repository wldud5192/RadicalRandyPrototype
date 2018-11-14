using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

	public float walkSpeed;
	public float runSpeed;
	Vector2 inputDirection;
	Vector3 startPosition;

	public bool playerIsDetected;
	Animator anim;


	void Start()
	{
		walkSpeed = 6f;
		runSpeed = 9f;
		anim = gameObject.GetComponent<Animator>();
	}

	public float speed;
	public float doorDropPos;
	public float doorSpeed;
	GameObject[] Unlocked;

	void FixedUpdate()
	{
		float moveHorizontal = Input.GetAxisRaw("Horizontal");
		float moveVertical = Input.GetAxisRaw("Vertical");

		Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
		movement = movement.normalized;

		if (movement.magnitude > 0.0001f)
		{
			if (playerIsDetected)
			{
				anim.SetBool("isRunning", true);
			}
			else
			{
				anim.SetBool("isWalking", true);
			}

			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15F);
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
			transform.Translate(movement * runSpeed * Time.deltaTime, Space.World);
		}
		else
		{
			transform.Translate(movement * walkSpeed * Time.deltaTime, Space.World);
		}
	}



	// Unlocking Doors
	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == ("RedKey"))                         //Identify the current key you are inside of
		{
			Unlocked = GameObject.FindGameObjectsWithTag("RedDoor");    //Puts all Doors of that colour into an array
			foreach (GameObject Unlocked in Unlocked)
			{
				Vector3 basePos = Unlocked.transform.position;          // BasePos is given the current position of each door
				basePos.y = doorDropPos;                                        // BasePos is then lowered to be (presumably) below field
				if (Unlocked.transform.position.y > basePos.y)          // if the current position is above the basepos then the door is lowered
				{
					Unlocked.transform.position = Vector3.MoveTowards(Unlocked.transform.position, basePos, doorSpeed);
				}
			}
		}
		else if (other.gameObject.tag == ("BlueKey"))                   //Same as applies above
		{
			Unlocked = GameObject.FindGameObjectsWithTag("BlueDoor");
			foreach (GameObject Unlocked in Unlocked)
			{
				Vector3 basePos = Unlocked.transform.position;
				basePos.y = doorDropPos;
				if (Unlocked.transform.position.y > basePos.y)
				{
					Unlocked.transform.position = Vector3.MoveTowards(Unlocked.transform.position, basePos, doorSpeed);
				}
			}
		}
		else if (other.gameObject.tag == ("GreenKey"))                     //Same as applies above
		{
			Unlocked = GameObject.FindGameObjectsWithTag("GreenDoor");
			foreach (GameObject Unlocked in Unlocked)
			{
				Vector3 basePos = Unlocked.transform.position;
				basePos.y = doorDropPos;
				if (Unlocked.transform.position.y > basePos.y)
				{
					Unlocked.transform.position = Vector3.MoveTowards(Unlocked.transform.position, basePos, doorSpeed);
				}
			}
		}
	}
}