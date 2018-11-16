#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using System.IO;

public class UI_SpeedMapGen : EditorWindow
{
	int MapSize_X, MapSize_Y;
	TextAsset mapPhysicalData;
	float tileOffset;
	Vector3 spawnOffset;

	GameObject masterList;

	[MenuItem("Tools/Map Generator V2")]
	public static void ShowWindow()
	{
		EditorWindow window = GetWindow(typeof(UI_SpeedMapGen));
		window.titleContent = new GUIContent("Map Generation Tool 2.0");
		window.Show();
	}

	private void OnGUI()
	{
		EditorGUILayout.BeginHorizontal();
		{
			EditorGUILayout.LabelField("Res X");
			MapSize_X = EditorGUILayout.IntField(MapSize_X);
			//int.TryParse(EditorGUILayout.TextArea("Res X"), out MapRes_X);
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		{
			EditorGUILayout.LabelField("Res Y");
			MapSize_Y = EditorGUILayout.IntField(MapSize_Y);

			//int.TryParse(EditorGUILayout.TextArea(MapRes_Y), out MapRes_Y);
		}
		EditorGUILayout.EndHorizontal();

		tileOffset = EditorGUILayout.FloatField(tileOffset);

		mapPhysicalData = (TextAsset)EditorGUILayout.ObjectField(mapPhysicalData, typeof(TextAsset));

		EditorGUILayout.BeginHorizontal();
		{
			if (GUILayout.Button("Load Map"))
			{
				ReadFile();
			}

			if (GUILayout.Button("Get Map Scale"))
			{
				PullMapScale();
			}
		}
		EditorGUILayout.EndHorizontal();
	}

	void ReadFile()
	{
		PullMapScale();

		GameObject generatedMap = GameObject.Find("Master List of Level Objects");
		DestroyImmediate(generatedMap);

		masterList = new GameObject();
		masterList.name = "Master List of Level Objects";
		spawnOffset = Vector2.zero;
		StreamReader FileRead = new StreamReader(AssetDatabase.GetAssetPath(mapPhysicalData));
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

				switch (FileDataReader[y][x])
				{
					case '○':
						Instantiate(Resources.Load<GameObject>("Floor"), spawnOffset, Quaternion.identity, masterList.transform);
					break;

					case '▨':
						Instantiate(Resources.Load<GameObject>("Wall"), spawnOffset, Quaternion.identity, masterList.transform);
					break;

					case '☆':
						Instantiate(Resources.Load<GameObject>("Player"), spawnOffset, Quaternion.identity, masterList.transform);
						Instantiate(Resources.Load<GameObject>("Floor"), spawnOffset, Quaternion.identity, masterList.transform);
					break;

					case '★':
						Instantiate(Resources.Load<GameObject>("Exit"), spawnOffset, Quaternion.identity, masterList.transform);
					break;

					case '◎':
						Instantiate(Resources.Load<GameObject>("Tile"), spawnOffset, Quaternion.identity, masterList.transform);
					break;

					case '※':
						Instantiate(Resources.Load<GameObject>("VPN"), spawnOffset, Quaternion.identity, masterList.transform);
						break;

					case '◐':
						Instantiate(Resources.Load<GameObject>("Switch"), spawnOffset, Quaternion.identity, masterList.transform);
						break;

					case '>':
						Instantiate(Resources.Load<GameObject>("AgentSpawn"), spawnOffset, Quaternion.identity, masterList.transform);
						Instantiate(Resources.Load<GameObject>("Floor"), spawnOffset, Quaternion.identity, masterList.transform);
						break;

					case 'V':
						Instantiate(Resources.Load<GameObject>("Virus"), spawnOffset, Quaternion.identity, masterList.transform);
						Instantiate(Resources.Load<GameObject>("Floor"), spawnOffset, Quaternion.identity, masterList.transform);
						break;

					case 'A':
						Instantiate(Resources.Load<GameObject>("Antivirus"), spawnOffset, Quaternion.identity, masterList.transform);
						break;

					case '§':
						Instantiate(Resources.Load<GameObject>("Key"), spawnOffset, Quaternion.identity, masterList.transform);
						Instantiate(Resources.Load<GameObject>("Floor"), spawnOffset, Quaternion.identity, masterList.transform);
						break;

					case 'Ⅲ':
						Instantiate(Resources.Load<GameObject>("Door"), spawnOffset, Quaternion.identity, masterList.transform);
						Instantiate(Resources.Load<GameObject>("Floor"), spawnOffset, Quaternion.identity, masterList.transform);
						break;

					case '♥':
						Instantiate(Resources.Load<GameObject>("Health"), spawnOffset, Quaternion.identity, masterList.transform);
						Instantiate(Resources.Load<GameObject>("Floor"), spawnOffset, Quaternion.identity, masterList.transform);
						break;

					case '⇔':
						Instantiate(Resources.Load<GameObject>("Counterhack"), spawnOffset, Quaternion.identity, masterList.transform);
						Instantiate(Resources.Load<GameObject>("Floor"), spawnOffset, Quaternion.identity, masterList.transform);
						break;

					default:
					break;
				}

				spawnOffset += Vector3.right * tileOffset;
			}

			spawnOffset = new Vector3(0, 0, spawnOffset.z + tileOffset);
		}
	}

	void PullMapScale()
	{
		if (mapPhysicalData != null)
		{
			StreamReader FileRead = new StreamReader(AssetDatabase.GetAssetPath(mapPhysicalData));
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
#endif