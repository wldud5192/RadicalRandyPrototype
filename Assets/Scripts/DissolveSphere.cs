using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveSphere : MonoBehaviour {

    public GameObject player;
    Shader shader1;
    Shader shader2;
    Renderer rend;
    public bool hiding = false;

    Material mat;

    void Start()
    {
        rend = player.gameObject.GetComponent<Renderer>();
        shader1 = Shader.Find("XRay Shaders/Diffuse-XRay-Replaceable");
        shader2 = Shader.Find("DissolverShader/DissolveShader");
        mat = player.gameObject.GetComponent<Renderer>().material;
    }

    void Update()
    {
        if (hiding)
        {
            rend.material.shader = shader2;
            mat.SetFloat("_DissolveAmount", Mathf.Sin(Time.time) / 2 + 0.5f);
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            

                    hiding = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            rend.material.shader = shader1;
            hiding = false;
        }
    }
}