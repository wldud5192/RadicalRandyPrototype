using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSettings : MonoBehaviour
{

    AudioSource audio;
    public bool playerIsAtSpawn;
    bool playOnce = false;

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

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
            
            if (!playOnce)
            {
                audio.Play();
                playOnce = true;
            }
        RenderSettings.ambientLight = Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time, 0.5f));
    } else
        {
            RenderSettings.ambientLight = Color.black;
        }

    }
}
