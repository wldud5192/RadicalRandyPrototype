using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This scales the menu UI to the size of the camera. It stops the UI not drawing fullscreen. Don't touch this. Without it the UI looks like https://i.imgur.com/yDQoI5H.png
/// </summary>
public class UIMenuFixer : MonoBehaviour
{

	void Start()
	{
		this.transform.localScale = new Vector3(Camera.main.orthographicSize * 2 * Screen.width / Screen.height, Camera.main.orthographicSize * 2 * Screen.width / Screen.height, 1);
	}

}
