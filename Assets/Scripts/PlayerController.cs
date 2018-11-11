using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

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

}