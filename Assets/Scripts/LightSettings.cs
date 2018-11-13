using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSettings : MonoBehaviour
{


    public bool playerIsAtSpawn;


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerIsAtSpawn = true;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerIsAtSpawn = false;
        }
    }
    void Update()
    {
        if (playerIsAtSpawn) { 
        RenderSettings.ambientLight = Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time, 1));
    } else
        {
            RenderSettings.ambientLight = Color.black;
        }

    }
}
