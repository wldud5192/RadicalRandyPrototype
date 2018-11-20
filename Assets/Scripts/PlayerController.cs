using UnityEngine;
using System.Collections;
using UnityEngine.UI;


[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public GameObject dashEffect;
	float dashSpeed;
    int dashCheck;

    float dashTime;
    float startDashTime;
    int direction;
    
	Vector2 inputDirection;
	Vector3 startPosition;

	public bool playerIsDetected;
	Animator anim;
	Rigidbody playerRB;

    float doorDropPos;
    float doorSpeed;
    GameObject[] Unlocked;

    void Start()
	{
        startDashTime = 0.2f;
        dashTime = startDashTime;
		anim = GetComponent<Animator>();
		playerRB = GetComponent<Rigidbody>();

        transform.position += new Vector3(0,0.1f,0);

		dashSpeed = 11.5f;

        playerRB.constraints = RigidbodyConstraints.None;
		playerRB.constraints = RigidbodyConstraints.FreezeRotation;
        playerRB.useGravity = true;
    }
    

    void Update()
    {
        UpdateCharacterToDirection();

        if (direction == 0)
        {
            SetDirection();
        } else
        {
            if(dashTime <= 0)
            {
                direction = 0;
                dashTime = startDashTime;
                playerRB.velocity = Vector2.zero;

            }
            else if (dashTime > 0)
            {
                dashTime -= Time.deltaTime;
                if (dashCheck == 0)
                {
                    Dash();
                } else
                {
                    DoubleDash();
                }
            }
        }
        
		if (playerRB.velocity.magnitude > 1)
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
        }
		else
		{
        }

	}
    

    IEnumerator doubleDashCooler()
    {
        dashCheck++;
        yield return new WaitForSeconds(0.3f);
        dashCheck = 0;
    }

    void UpdateCharacterToDirection()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);
        if (movement.magnitude != 0)
        {
            transform.rotation = Quaternion.LookRotation(movement);
        }
    }

    void SetDirection()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            DashEffect();
            direction = 1;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            DashEffect();
            direction = 2;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            DashEffect();
            direction = 3;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            DashEffect();
            direction = 4;
        }

    }

    void DashEffect()
    {
        Instantiate(dashEffect, transform.position, Quaternion.identity);
        GameObject clone = Instantiate(dashEffect, transform.position, Quaternion.identity);
        Destroy(clone, 1.5f);
    }

    void Dash()
    {
        StartCoroutine(doubleDashCooler());
        
        if (direction == 1)
        {
            playerRB.velocity = Vector3.left * dashSpeed;
        }
        else if (direction == 2)
        {
            playerRB.velocity = Vector3.right * dashSpeed;
        }
        else if (direction == 3)
        {
            playerRB.velocity = Vector3.forward * dashSpeed;
        }
        else if (direction == 4)
        {
            playerRB.velocity = Vector3.back * dashSpeed;
        }

    }
    

    void DoubleDash()
    {
        Debug.Log("DD");

       
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