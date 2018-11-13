//Ignore this garbage for now. It works for generating maps but the loading is absolute shit so I replaced it with the UI_SpeedMapGen. If you can fix this then go ahead otherwise just scrap it, but make sure
//to grab the GenerateMap

/*using System.Collections;
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

	char[][] fileDataReader;

	MapObjectList_SO mapMeshData;
	TextAsset mapPhysicalData;

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

		mapMeshData = (MapObjectList_SO)EditorGUILayout.ObjectField(mapMeshData, typeof(MapObjectList_SO));
		mapPhysicalData = (TextAsset)EditorGUILayout.ObjectField(mapPhysicalData, typeof(TextAsset));

		if (GUILayout.Button("Generate Map"))
		{
			if (mapMeshData == null)
			{
				//Debug.logError("Failed to get mapData. Set Map Data!");
			}
			else
			{
				dataListTypes = string.Empty;
				GenerateMap(false);
			}
		}

		EditorGUILayout.BeginHorizontal();
		{
			if (GUILayout.Button("Load Map from File"))
			{
				LoadMap();
			}
			if (GUILayout.Button("Save Map to File"))
			{
				SaveMap();
			}
		}
		EditorGUILayout.EndHorizontal();
	}

	GameObject gridParent;

	void GenerateExternalLayer()
	{
		GameObject wall = Instantiate(mapMeshData.mapExterior);
		wall.transform.position = meshGridPosition;
		wall.name = "External Wall";
		wall.transform.parent = gridParent.transform;
		wall.isStatic = true;
	}

	void GenerateMap(bool readingFromFile)
	{
		GameObject generatedMap = GameObject.Find("Grid Parent");
		DestroyImmediate(generatedMap);

		meshGridPosition = Vector3.zero;

		gridParent = new GameObject();
		gridParent.name = "Grid Parent";

		//Scan on Y & X axis
		for (int y = 0; y < mapRes_Y; y++)
		{
			for (int x = 0; x < mapRes_X; x++)
			{
				//If we're on the first, last or any side layers then generate
				if (y == 0 || y == mapRes_Y - 1 || x == 0 || x == mapRes_X - 1)
				{
					GenerateExternalLayer();

					if (y != 0 && y < mapRes_Y - 2)
					{
						if (x == mapRes_X - 1)
						{
							dataListTypes += "\n";
						}
					}
				}
				else
				{
					if (readingFromFile)
					{
						GameObject innerEnvironmentPiece;
						//Debug.Log(y + " | " + x);
						if (y < mapRes_Y - 1)
							if (x < mapRes_X - 1)
								switch (fileDataReader[y][x])
								{
									case '\u25CB':
										innerEnvironmentPiece = Instantiate(mapMeshData.meshObjects[0]);
										innerEnvironmentPiece.transform.position = meshGridPosition;
										innerEnvironmentPiece.transform.parent = gridParent.transform;
										innerEnvironmentPiece.name = y + "," + x + " \u2327";
										//Debug.Log("Floor");
										break;

									case '\u25A8':
										innerEnvironmentPiece = Instantiate(mapMeshData.meshObjects[1]);
										innerEnvironmentPiece.transform.position = meshGridPosition;
										innerEnvironmentPiece.transform.parent = gridParent.transform;
										innerEnvironmentPiece.name = y + "," + x + " \u25A8";
										//Debug.Log("Wall");
										break;
								}

					}
					else
					{
						int randomID = Random.Range(0, mapMeshData.meshObjects.Length);
						GameObject innerEnviornment = Instantiate(mapMeshData.meshObjects[randomID]);
						innerEnviornment.transform.position = meshGridPosition;

						switch (mapMeshData.meshObjects[randomID].name.ToLower())
						{
							case "floor":
								dataListTypes += "\u25CB";
								break;

							case "wall":
								dataListTypes += "\u25A8";
								break;

							case "start":
								dataListTypes += "☆";

								break;

							case "exit":
								dataListTypes += "★";
								break;

							case "cpu":
								dataListTypes += "◎";
								break;

							case "vpn":
								dataListTypes += "※";
								break;

							case "switch":
								dataListTypes += "◐";
								break;

							case "agentspawn_left":
								dataListTypes += ">";
								break;

							case "agentspawn_right":
								dataListTypes += "<";
								break;

							case "virus":
								dataListTypes += "V";
								break;

							case "antivirus":
								dataListTypes += "A";
								break;

							case "key":
								dataListTypes += "§";
								break;

							case "door":
								dataListTypes += "Ⅲ";
								break;

							case "health":
								dataListTypes += "♥";
								break;

							case "counterhack":
								dataListTypes += "⇔";
								break;

							default:
								break;
						}

						innerEnviornment.name = mapMeshData.meshObjects[randomID].name.ToLower();

						innerEnviornment.transform.parent = gridParent.transform;
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
			//Debug.logError("Please save this map first.");
			return;
		}

		UnityEditor.AI.NavMeshBuilder.BuildNavMesh();
		SaveMap();
	}

	void SaveMap()
	{
		string fileWriteDir = "";
		if (mapPhysicalData == null)
		{
			string MapLocation = Application.dataPath + "//Scenes//GeneratedFileData//";
			string mapName = SceneManager.GetActiveScene().name;

			mapName = mapName.Replace(" ", "_");
			mapName = mapName.Replace(".", "_");
			dataListTypes = dataListTypes.Replace(" ", "");
			fileWriteDir = MapLocation + mapName + ".txt";
		}
		else
		{
			fileWriteDir = AssetDatabase.GetAssetPath(mapPhysicalData);
			//Debug.Log(fileWriteDir);
		}

		File.WriteAllText(fileWriteDir, dataListTypes);
	}

	void LoadMap()
	{
		string mapFilePath = "";
		string mapFileData = "";

		if (mapPhysicalData == null)
		{
			string mapLocation = Application.dataPath + "//Scenes//GeneratedFileData//";
			string mapName = SceneManager.GetActiveScene().name;

			mapName = mapName.Replace(" ", "_");
			mapName = mapName.Replace(".", "_");

			mapFilePath = mapLocation + mapName + ".txt";
		}
		else
		{
			mapFilePath = AssetDatabase.GetAssetPath(mapPhysicalData);
		}

		try
		{
			StreamReader fileRead = new StreamReader(mapFilePath);
			mapFileData = fileRead.ReadToEnd();
		}
		catch (System.Exception)
		{
			//Debug.Log("Failed to find File at " + mapFilePath);
		}

		int i = 0, j = 0;
		fileDataReader = new char[mapRes_Y - 2][];

		foreach (var row in mapFileData.Split('\n'))
		{
			fileDataReader[j] = row.ToCharArray();
			Debug.Log("ROW DATA: " + row.ToString());
			j++;
		}

		for (int y = 1; y < mapRes_Y - 2; y++)
		{
			for (int x = 1; x < mapRes_X - 1; x++)
			{
				Debug.Log(fileDataReader[y][x]);
			}
		}

		GenerateMap(true);
	}
}*/