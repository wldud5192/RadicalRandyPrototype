using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed;

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
}