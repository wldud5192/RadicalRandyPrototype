using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGrid : MonoBehaviour
{

    public GameObject[] targets;
    public GameObject[] structure;
    Vector3 truePos;
    public float gridSize;
    

    void LateUpdate()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            truePos.x = Mathf.Floor(targets[i].transform.position.x / gridSize) * gridSize;
            truePos.y = Mathf.Floor(targets[i].transform.position.y / gridSize) * gridSize;
            truePos.z = Mathf.Floor(targets[i].transform.position.z / gridSize) * gridSize;

            structure[i].transform.position = truePos;
        }
    }

}