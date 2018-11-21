using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class ExitScript : MonoBehaviour {
    
    GameObject[] listOfCpuTiles;
    int startNum;
    public int currentNum;
    public int sceneNumber;
    bool alreadyPlayed = false;
    public bool exitOpen = false;
    public bool sceneIsBuilt = false;


	// Use this for initialization
	void Start () {
        
        Component[] destroyOnLoad = GetComponents(this.GetType());
        if (destroyOnLoad.Length > 1)
        {
            Destroy(destroyOnLoad[0]);

        }

        if (GameObject.FindGameObjectsWithTag("CPU") == null)
        {
            sceneNumber = 0;
        } else {
            listOfCpuTiles = GameObject.FindGameObjectsWithTag("CPU");
        }

        startNum = listOfCpuTiles.Length;
        currentNum = startNum;
        sceneNumber = SceneManager.GetActiveScene().buildIndex;


    }
	
	// Update is called once per frame
	void Update () {


        if (currentNum == 0)
        {

            exitOpen = true;
        }

        if (exitOpen)

        {

            transform.position = new Vector3(transform.position.x, Mathf.PingPong(Time.time * 2, 0.5f) - 0.5f / 2f, transform.position.z);


            if (!alreadyPlayed)
            {
                GetComponent<AudioSource>().Play();            }

            alreadyPlayed = true;
        }  


        if(Input.GetKeyDown(KeyCode.H))
        {
            int newSceneNum = (sceneNumber += 1);
            SceneManager.LoadScene(newSceneNum);

        }

        if(Input.GetKeyDown(KeyCode.G))
        {
            int newSceneNum = (sceneNumber -= 1);
            SceneManager.LoadScene(newSceneNum);
        }
 	}


    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player" && exitOpen && !sceneIsBuilt)
        {
            int newSceneNum = sceneNumber += 1;
            SceneManager.LoadScene(newSceneNum);
            sceneIsBuilt = true;

            //DestroyAllGameObjects();
        }
    }

        public void DestroyAllGameObjects()
    {
        GameObject[] GameObjects = (FindObjectsOfType<GameObject>() as GameObject[]);

        for (int i = 0; i < GameObjects.Length; i++)
        {
            Destroy(GameObjects[i]);
        }
    }

}
