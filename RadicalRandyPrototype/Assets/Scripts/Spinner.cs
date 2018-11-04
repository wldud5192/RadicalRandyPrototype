using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spinner : MonoBehaviour
{
    public Transform centre;
    Quaternion startingCamRot;
    public Vector3 RotationPerSecond;
    float inputTimer = 0;

    void Start()
    {
        startingCamRot = gameObject.transform.rotation;
    }

    void Update()
    {
        inputTimer += Time.deltaTime;

        if (Input.anyKey)
        {
            //Can we do this a bit more smoothly? + set anchor point more accurate to the centre of the level?
            gameObject.transform.SetPositionAndRotation(transform.position, startingCamRot);
            inputTimer = 0;
        }

        if (inputTimer >= 5f)
        {
            transform.position = centre.transform.position;
            transform.Rotate(RotationPerSecond * Time.deltaTime);

        }
    }
}