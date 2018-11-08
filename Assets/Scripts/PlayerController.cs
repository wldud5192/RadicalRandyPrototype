using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed;
    GameObject[] Unlocked;

    void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		if (movement.magnitude > 0.0001f) {
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (movement), 0.15F);
		}
		transform.Translate (movement * speed * Time.deltaTime, Space.World);
	}


    // Unlocking Doors
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == ("RedKey"))                         //Identify the current key you are inside of
        {
            Unlocked = GameObject.FindGameObjectsWithTag("RedDoor");    //Puts all Doors of that colour into an array
            foreach (GameObject Unlocked in Unlocked)                 
            {
                Vector3 BasePos = Unlocked.transform.position;          // BasePos is given the current position of each door
                BasePos.y = -6f;                                        // BasePos is then lowered to be (presumably) below field
                if (Unlocked.transform.position.y > BasePos.y)          // if the current position is above the basepos then the door is lowered
                {
                    Unlocked.transform.position = Vector3.MoveTowards(Unlocked.transform.position, BasePos, 0.5f);
                }
            }
        }
        else if (other.gameObject.tag == ("BlueKey"))                   //Same as applies above
        {
            Unlocked = GameObject.FindGameObjectsWithTag("BlueDoor");
            foreach (GameObject Unlocked in Unlocked)
            {
                Vector3 BasePos = Unlocked.transform.position;
                BasePos.y = -6f;
                if (Unlocked.transform.position.y > BasePos.y)
                {
                    Unlocked.transform.position = Vector3.MoveTowards(Unlocked.transform.position, BasePos, 0.5f);
                }
            }
        }
        else if (other.gameObject.tag == ("GreenKey"))                     //Same as applies above
        {
            Unlocked = GameObject.FindGameObjectsWithTag("GreenDoor");
            foreach (GameObject Unlocked in Unlocked)
            {
                Vector3 BasePos = Unlocked.transform.position;
                BasePos.y = -6f;
                if (Unlocked.transform.position.y > BasePos.y)
                {
                    Unlocked.transform.position = Vector3.MoveTowards(Unlocked.transform.position, BasePos, 0.5f);
                }
            }
        }
    }
}