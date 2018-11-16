using UnityEngine;
using System.Collections;
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

	void Start()
	{
		anim = gameObject.GetComponent<Animator>();
		playerRB = GetComponent<Rigidbody>();

        transform.position += new Vector3(0,0.1f,0);

		walkSpeed = 500;
		runSpeed = 900;

        playerRB.constraints = RigidbodyConstraints.None;
        playerRB.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        playerRB.useGravity = true;

    }

    public float speed;
	public float doorDropPos;
	public float doorSpeed;
	GameObject[] Unlocked;

	void Update()
	{
		float moveHorizontal = Input.GetAxisRaw("Horizontal");
		float moveVertical = Input.GetAxisRaw("Vertical");

		Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15F);
		movement = movement.normalized;

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