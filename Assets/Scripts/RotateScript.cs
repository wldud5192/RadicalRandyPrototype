using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScript : MonoBehaviour
{

    public float speed;
    string direction;

    void Update()
    {
        transform.Rotate(Vector3.right * speed * Time.deltaTime);
    }
}
