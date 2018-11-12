using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEditor;
using UnityEditor.AI;
using UnityEngine.SceneManagement;

/// <summary>
/// Generates a map based on X and Y scale, sets and builds NavMesh and inputs Mapdata and scale.
/// </summary>
public class UI_MapGenerator : EditorWindow
{
	int mapRes_X;
	int mapRes_Y;

	float objectScale = 2.0f;

	Vector3 meshGridPosition;
	int meshPieceID;
	string dataListTypes;

	int[][] fileDataReader;

	MapObjectList_SO mapData;

	[MenuItem("Tools/Map Generator")]
	public static void ShowWindow()
	{
		EditorWindow window = GetWindow(typeof(UI_MapGenerator));
		window.titleContent = new GUIContent("Map Generation Tool");
		window.Show();
	}

	private void OnGUI()
	{
		EditorGUILayout.BeginHorizontal();
		{
			EditorGUILayout.LabelField("Res X");
			mapRes_X = EditorGUILayout.IntField(mapRes_X);
			//int.TryParse(EditorGUILayout.TextArea("Res X"), out MapRes_X);
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		{
			EditorGUILayout.LabelField("Res Y");
			mapRes_Y = EditorGUILayout.IntField(mapRes_Y);

			//int.TryParse(EditorGUILayout.TextArea(MapRes_Y), out MapRes_Y);
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		{
			EditorGUILayout.LabelField("Object Scale");
			objectScale = EditorGUILayout.FloatField(objectScale);
		}
		EditorGUILayout.EndHorizontal();

		mapData = (MapObjectList_SO)EditorGUILayout.ObjectField(mapData, typeof(MapObjectList_SO));

		if (GUILayout.Button("Generate Map"))
		{
			if (mapData == null)
			{
				Debug.LogError("Failed to get mapData. Set Map Data!");
			}
			else
			{
				dataListTypes = string.Empty;
				GenerateMap(false);
			}
		}

		if(GUILayout.Button("Load Map from File"))
		{
			LoadMap();
		}


	}

	void GenerateMap(bool readingFromFile)
	{
		GameObject generatedMap = GameObject.Find("Grid Parent");
		DestroyImmediate(generatedMap);

		meshGridPosition = Vector3.zero;
		meshPieceID = 1;

		GameObject ParentCube = new GameObject();
		ParentCube.name = "Grid Parent";

		for (int y = 0; y < mapRes_Y; y++)
		{
			for (int x = 0; x < mapRes_X; x++)
			{
				if (y == 0 || y == mapRes_Y - 1 || x == 0 || x == mapRes_X - 1)
				{
					dataListTypes += -1 + ", ";
					if (x == mapRes_X - 1)
					{
						dataListTypes += "\r\n";
					}
					GameObject wall = Instantiate(mapData.mapExterior);
					wall.transform.position = meshGridPosition;
					wall.name = meshPieceID.ToString();
					wall.transform.parent = ParentCube.transform;
					wall.isStatic = true;
				}
				else
				{
					if (readingFromFile)
					{
						GameObject innerEnvironmentPiece = Instantiate(mapData.meshObjects[fileDataReader[y][x]]);
					}
					else
					{
						int randomID = Random.Range(0, mapData.meshObjects.Length);
						dataListTypes += randomID + ", ";

						GameObject innerEnviornment = Instantiate(mapData.meshObjects[randomID]);
						innerEnviornment.transform.position = meshGridPosition;
						innerEnviornment.name = meshPieceID.ToString();
						innerEnviornment.transform.parent = ParentCube.transform;
						innerEnviornment.isStatic = true;

						if (!innerEnviornment.transform.GetChild(0).gameObject.name.ToLower().Contains("wall"))
						{
							GameObjectUtility.SetStaticEditorFlags(innerEnviornment.transform.GetChild(0).gameObject, StaticEditorFlags.NavigationStatic);
							GameObjectUtility.SetNavMeshArea(innerEnviornment.transform.GetChild(0).gameObject, 0);
						}
					}
				}

				meshGridPosition += Vector3.right * objectScale;					
				meshPieceID++;
			}

			meshGridPosition = new Vector3(0, 0, meshGridPosition.z + objectScale);
		}

		if (SceneManager.GetActiveScene().name == "")
		{
			Debug.LogError("Please save this map first.");
			return;
		}

		UnityEditor.AI.NavMeshBuilder.BuildNavMesh();
		SaveMap();
	}

	void SaveMap()
	{
		string MapLocation = Application.dataPath + "//Scenes//GeneratedFileData//";
		string MapName = SceneManager.GetActiveScene().name;

		File.WriteAllText(MapLocation + MapName + ".txt", dataListTypes);
	}

	void LoadMap()
	{
		string MapLocation = Application.dataPath + "//Scenes//GeneratedFileData//";
		string MapName = SceneManager.GetActiveScene().name;
		MapName = MapName.Replace(" ", "_");
		string MapFilePath = MapLocation + MapName;

		try
		{
			StreamReader fileRead = new StreamReader(MapFilePath);
		}
		catch (System.Exception)
		{
			Debug.Log("Failed to find File " + MapName + " in " + MapLocation);
		}
	}
}
