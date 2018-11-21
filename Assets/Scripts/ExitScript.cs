using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class ExitScript : MonoBehaviour
{
	DestroyableTile[] listOfCpuTiles;
	int startNum;
	public int currentNum;
	public int sceneNumber;
	bool alreadyPlayed = false;
	public bool exitOpen = false;
	public bool sceneIsBuilt = false;

	void Start()
	{
		ExitScript[] excessExitTiles = GameObject.FindObjectsOfType<ExitScript>();

		if (excessExitTiles.Length > 1)
		{
			for (int i = 1; i < excessExitTiles.Length; i++)
			{
				Destroy(excessExitTiles[i]);
			}
		}

		listOfCpuTiles = GameObject.FindObjectsOfType<DestroyableTile>();

		startNum = listOfCpuTiles.Length;
		Debug.Log("START NUMBER OF TILES IS " + startNum);
		currentNum = startNum;
		sceneNumber = SceneManager.GetActiveScene().buildIndex;
	}

	void Update()
	{
		if (currentNum == 0)
		{
			exitOpen = true;
		}

		if (exitOpen)
		{
			transform.position = new Vector3(transform.position.x, Mathf.PingPong(Time.time * 2, 0.5f) - 0.5f / 2f, transform.position.z);

			if (!alreadyPlayed)
			{
				GetComponent<AudioSource>().Play();
			}
			alreadyPlayed = true;
		}

		sceneNumber = Mathf.Clamp(sceneNumber, 0, 20);

		if (Input.GetKeyDown(KeyCode.H))
		{
			int newSceneNum = (sceneNumber += 1);
			SceneManager.LoadScene(newSceneNum);
		}

		if (Input.GetKeyDown(KeyCode.G))
		{
			int newSceneNum = (sceneNumber -= 1);
			SceneManager.LoadScene(newSceneNum);
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player" && exitOpen && !sceneIsBuilt)
		{
			int newSceneNum = sceneNumber += 1;
			SceneManager.LoadScene(newSceneNum);
			sceneIsBuilt = true;

			DestroyAllGameObjects();
		}
	}

	public void DestroyAllGameObjects()
	{
		GameObject[] GameObjects = (FindObjectsOfType<GameObject>() as GameObject[]);

		for (int i = 0; i < GameObjects.Length; i++)
		{
			Destroy(GameObjects[i]);
		}
	}
}
