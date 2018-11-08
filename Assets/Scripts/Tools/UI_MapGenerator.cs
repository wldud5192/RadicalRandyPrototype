﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AI;


/// <summary>
/// Generates a map based on X and Y scale, sets and builds NavMesh and inputs Mapdata and scale.
/// </summary>
public class UI_MapGenerator : EditorWindow
{
	int MapRes_X;
	int MapRes_Y;

	float objectScale = 2.0f;

	Vector3 CubePosition;
	int CubeName;

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
			MapRes_X = EditorGUILayout.IntField(MapRes_X);
			//int.TryParse(EditorGUILayout.TextArea("Res X"), out MapRes_X);
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		{
			EditorGUILayout.LabelField("Res Y");
			MapRes_Y = EditorGUILayout.IntField(MapRes_Y);

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
				GenerateMap();
			}
		}
	}

	void GenerateMap()
	{
		GameObject generatedMap = GameObject.Find("Grid Parent");
		DestroyImmediate(generatedMap);

		CubePosition = Vector3.zero;

		CubeName = 1;

		GameObject ParentCube = new GameObject();
		ParentCube.name = "Grid Parent";

		for (int y = 0; y < MapRes_Y; y++)
		{
			for (int x = 0; x < MapRes_X; x++)
			{
				if (y == 0 || y == MapRes_Y - 1 || x == 0 || x == MapRes_X - 1)
				{

					GameObject wall = Instantiate(mapData.mapExterior);
					wall.transform.position = CubePosition;
					wall.name = CubeName.ToString();
					wall.transform.parent = ParentCube.transform;
					wall.isStatic = true;

					//GameObjectUtility.SetStaticEditorFlags(wall.transform.GetChild(0).gameObject, StaticEditorFlags.NavigationStatic);
					//GameObjectUtility.SetNavMeshArea(wall, 1);
				}
				else
				{
					GameObject innerEnviornment = Instantiate(mapData.meshObjects[Random.Range(0, mapData.meshObjects.Length)]);
					innerEnviornment.transform.position = CubePosition;
					innerEnviornment.name = CubeName.ToString();
					innerEnviornment.transform.parent = ParentCube.transform;
					innerEnviornment.isStatic = true;

					if (!innerEnviornment.transform.GetChild(0).gameObject.name.ToLower().Contains("wall"))
					{
						GameObjectUtility.SetStaticEditorFlags(innerEnviornment.transform.GetChild(0).gameObject, StaticEditorFlags.NavigationStatic);
						GameObjectUtility.SetNavMeshArea(innerEnviornment.transform.GetChild(0).gameObject, 0);
					}
				}


				CubePosition += Vector3.right * objectScale;

				CubeName++;
			}

			CubePosition = new Vector3(0, 0, CubePosition.z + objectScale);
		}
		UnityEditor.AI.NavMeshBuilder.BuildNavMesh();
	}
}
