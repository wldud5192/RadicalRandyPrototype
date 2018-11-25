using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveSphere : MonoBehaviour
{

    GameObject player;
    AudioSource audio;
    Renderer rend;
    public bool hiding = false;
    float cooldown;
    public bool onCD = false;

    float duration = 0.3f;

    Material mat;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        player = GameObject.Find("Character");
        rend = player.GetComponent<SkinnedMeshRenderer>();
    }

    void Update()
    {

        if (hiding)
        {
            float lerp = Mathf.PingPong(Time.time, duration) / duration;
            rend.material.color = Color.Lerp(Color.red, Color.green, lerp);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            hiding = true;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" /*&& !onCD*/)
        {
            audio.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" /* && onCD*/)
        {
            hiding = false;
            rend.material.color = Color.white;
        }
    }

    
}