using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

<<<<<<< HEAD
	public float walkSpeed;
    public float runSpeed;
    public int lives;
    Vector2 inputDirection;
    Vector3 startPosition;

    public Image gameOverUI;

    public bool playerIsDetected;
    Animator anim;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }
=======
	public float speed;
    public float doorDropPos;
    public float doorSpeed;
    GameObject[] Unlocked;
>>>>>>> origin/Player

    void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		if (movement.magnitude > 0.0001f) {
            if (playerIsDetected)
            { anim.SetBool("isRunning", true);
            }
            else
            { anim.SetBool("isWalking", true);
            }
            			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (movement), 0.15F);
		} else
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
        } else
        {
            transform.Translate(movement * walkSpeed * Time.deltaTime, Space.World);
        }
	}

<<<<<<< HEAD
    void OnPlayerKilled ()
    {
        if (lives - 1 > 0)
        {
            lives -= 1;
        }

        else

        {

            gameOverUI.gameObject.SetActive(true);

        }

    }

    public void OnGameOverButtonClick()
    {
        //reset scene
    }

=======

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
>>>>>>> origin/Player
}