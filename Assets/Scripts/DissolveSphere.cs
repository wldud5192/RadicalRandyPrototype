using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveSphere : MonoBehaviour
{

    GameObject player;
    AudioSource audio;
    Shader shader1;
    Shader shader2;
    Renderer rend;
    public bool hiding = false;
    bool exiting;
    float cooldown;
    //public bool onCD = false;

    Material mat;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        player = GameObject.Find("Character");
        rend = player.GetComponent<SkinnedMeshRenderer>();
        shader1 = Shader.Find("XRay Shaders/Diffuse-XRay-Replaceable");
        shader2 = Shader.Find("DissolverShader/DissolveShader");
        mat = rend.material;
    }

    void Update()
    {
        if (hiding)
        {
            mat.shader = shader2;
        }
        else
        {
            mat.shader = shader1;
        }

        if (mat.shader == shader2)
        {
            mat.SetFloat("_DissolveAmount", Mathf.Lerp(0, 60, Time.deltaTime));
        }

        /*if (onCD)
        {
            cooldown -= Time.deltaTime;
            if (cooldown < 0)
            {
               onCD = false;
            }
        }*/
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
            cooldown = 10;
            audio.Play();
            //onCD = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" /* && onCD*/)
        {
            exiting = true;
            hiding = false;

        }
    }

    
}