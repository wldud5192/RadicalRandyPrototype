using UnityEngine;
using System.Collections;


/// <summary>
///  Create a scene from scratch
/// </summary>
public class SceneFromScratch : MonoBehaviour
{

    /*
    // Use this for initialization
    void Start()
    {
        // create a camera
        GameObject cam = new GameObject();
        cam.name = "Generated Camera";
        cam.transform.parent = this.gameObject.transform;
        cam.AddComponent<Camera>();
        cam.transform.position = new Vector3(2.5f, 2, -3);
        cam.transform.localEulerAngles = new Vector3(20, 325, 5);

        // create a light
        GameObject lightGameObject = new GameObject("Generated Light");
        lightGameObject.transform.parent = this.gameObject.transform;
        lightGameObject.AddComponent<Light>();
        lightGameObject.GetComponent<Light>().color = Color.white;
        lightGameObject.transform.position = new Vector3(-0.8f, 1.3f, -1.4f);

        // create a generic cube
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.name = "Generated Cube";
        cube.transform.parent = this.gameObject.transform;

        // create a custom square
        GameObject g = GetSquareGameObject("Generated Square");
        g.transform.parent = this.gameObject.transform;
        g.transform.localScale = new Vector3(2, 2, 2);

    }

    // Update is called once per frame
    void Update()
    {

    }


    GameObject GetSquareGameObject(string name)
    {
        GameObject g;

        g = new GameObject(name);
        g.AddComponent<MeshFilter>();
        g.AddComponent<MeshRenderer>();

        // assign a mesh
        g.GetComponent<MeshFilter>().mesh = GetSquareMesh();

        // modify material (and shader)
        g.GetComponent<Renderer>().material.color = Color.red;

        Shader shader = Shader.Find("Diffuse");
        g.GetComponent<Renderer>().material.shader = shader;


        return g;
    }

    Mesh GetSquareMesh()
    {
        Vector3[] newVertices = new Vector3[4];
        Vector3[] newNormals = new Vector3[4];
        Vector2[] newUV = new Vector2[4];
        int[] newTriangles = new int[6];

        Mesh mesh = new Mesh();

        // vertices
        newVertices[0] = new Vector3(0, 0, 0);
        newVertices[1] = new Vector3(0, 1, 0);
        newVertices[2] = new Vector3(1, 0, 0);
        newVertices[3] = new Vector3(1, 1, 0);

        // normals
        newNormals[0] = -Vector3.forward;
        newNormals[1] = -Vector3.forward;
        newNormals[2] = -Vector3.forward;
        newNormals[3] = -Vector3.forward;

        // uv
        newUV[0] = new Vector2(0, 0);
        newUV[1] = new Vector2(0, 1);
        newUV[2] = new Vector2(1, 0);
        newUV[3] = new Vector2(1, 1);

        // triangles

        // first triangle
        newTriangles[0] = 0;
        newTriangles[1] = 1;
        newTriangles[2] = 2;

        // second triangle
        newTriangles[3] = 1;
        newTriangles[4] = 3;
        newTriangles[5] = 2;

        // assign to mesh
        mesh.vertices = newVertices;
        mesh.uv = newUV;
        mesh.normals = newNormals;
        mesh.triangles = newTriangles;

        return mesh;
    }
     */

    }