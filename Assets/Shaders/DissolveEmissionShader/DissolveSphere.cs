using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveSphere : MonoBehaviour {
    Shader shader1;
    Shader shader2;
    Renderer rend;
    bool hiding = false;

    Material mat;

    void Start()
    {
        rend = GetComponent<Renderer>();
        shader1 = Shader.Find("XRay Shaders/Diffuse-XRay-Replaceable");
        shader2 = Shader.Find("DissolverShader/DissolveShader");
        mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        if (hiding)
        {
            mat.SetFloat("_DissolveAmount", Mathf.Sin(Time.time) / 2 + 0.5f);
        }
        if (Input.GetKeyDown(KeyCode.G))
            if (rend.material.shader == shader1)
            {
                rend.material.shader = shader2;
                {
                    hiding = true;
                }
            }
            else
            {
                rend.material.shader = shader1;
            }
    }
}