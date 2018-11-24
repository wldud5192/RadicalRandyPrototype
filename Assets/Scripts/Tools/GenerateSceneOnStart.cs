using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GenerateSceneOnStart : MonoBehaviour
{
	int MapSize_X, MapSize_Y;
	string mapDirectory;
	float tileOffset;
	Vector3 spawnOffset;
	string directory;
	GameObject masterList;

	private void Start()
	{
		tileOffset = 2.0f;

		mapDirectory = SceneManager.GetActiveScene().name + ".txt";
		directory = Application.dataPath + "//..//StreamingAssets/";
		Debug.Log(directory);

		ReadFile();
	}

	void ReadFile()
	{
		PullMapScale();

		GameObject generatedMap = GameObject.Find("Master List of Level Objects");
		DestroyImmediate(generatedMap);

		masterList = new GameObject();
		masterList.name = "Master List of Level Objects";
		spawnOffset = Vector2.zero;
		StreamReader FileRead = new StreamReader(directory + mapDirectory);
		string MapData = FileRead.ReadToEnd();

		int j = 0;

		char[][] FileDataReader = new char[MapSize_Y][];
		foreach (string row in MapData.Split('\n'))
		{
			FileDataReader[j] = row.ToCharArray();
			j++;
		}

		for (int y = 0; y < MapSize_Y; y++)
		{
			for (int x = 0; x < MapSize_X; x++)
			{
				Debug.Log(FileDataReader[y][x]);

				GameObject Item = null;

				switch (FileDataReader[y][x])
				{
					case '○':
						Item = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>("Floor"));
						break;

					case '▨':
						Item = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>("Wall"));
						break;

					case '☆':
						if(GameObject.FindObjectOfType<PlayerController>() != null)
						{
							GameObject.FindObjectOfType<PlayerController>().gameObject.transform.position = new Vector3(x, 0.1f, y);
						}
						else
						{
							Item = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>("Player"));
							Item.transform.position = new Vector3(x, 0.1f, y);
						}
						Item = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>("Floor"));
						break;

					case '★':
						Item = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>("Exit"));
						break;

					case '◎':
						Item = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>("Tile"));
						break;

					case '※':
						Item = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>("VPN"));
						break;

					case '◐':
						Item = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>("Switch"));
						break;

					case '>':
						Item = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>("AgentSpawn"));
						Item = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>("Floor"));
						break;

					case 'V':
						Item = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>("Virus"));
						Item = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>("Floor"));
						break;

					case 'A':
						Item = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>("Antivirus"));
						break;

					case '§':
						Item = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>("Key"));
						Item = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>("Floor"));
						break;

					case 'Ⅲ':
						Item = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>("Door"));
						Item = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>("Floor"));
						break;

					case '♥':
						Item = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>("Health"));
						Item = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>("Floor"));
						break;

					case '⇔':
						Item = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>("Counterhack"));
						Item = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>("Floor"));
						break;

					default:
						Item = new GameObject();
						break;
				}

				Item.transform.position = spawnOffset;
				Item.transform.rotation = Quaternion.identity;
				Item.transform.parent = masterList.transform;

				spawnOffset += Vector3.right * tileOffset;
			}

			spawnOffset = new Vector3(0, 0, spawnOffset.z + tileOffset);
		}

		GameObject Nav = GameObject.FindObjectOfType<LocalNavMeshBuilder>().gameObject;
		DestroyImmediate(Nav);
		Nav = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>("NavmeshBuilder"));
		Nav.transform.position = Vector3.zero;
		Nav.GetComponent<LocalNavMeshBuilder>().m_Size = Vector3.one * 100;
	}

	void SetItemPositioning(GameObject Item)
	{
		if (Item != null)
		{
			Item.transform.position = spawnOffset;
			Item.transform.rotation = Quaternion.identity;
			Item.transform.parent = masterList.transform;
		}
		else
		{
			Debug.Log("Failed to get an object!");
		}
	}

	void PullMapScale()
	{
		if (mapDirectory != null)
		{
			StreamReader FileRead = new StreamReader(directory + mapDirectory);
			string MapData = FileRead.ReadToEnd();

			int yRes = 0;
			int xRes = 0;

			foreach (string row in MapData.Split('\n'))
			{
				yRes++;
			}

			for (int x = 0; x < MapData.Length; x++)
			{
				if (MapData[x + 1] == '\n')
				{
					break;
				}
				else
				{
					xRes++;
					Debug.Log(MapData[x]);
				}
			}

			MapSize_X = xRes;
			MapSize_Y = yRes;
		}
		else
		{
			Debug.LogError("Please input a Map txt file");
		}
	}
}
