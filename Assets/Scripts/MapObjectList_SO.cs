using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Map List SO", menuName = "Mapdata SO")]
public class MapObjectList_SO : ScriptableObject
{
	public GameObject mapExterior;
	public GameObject[] meshObjects;
}
