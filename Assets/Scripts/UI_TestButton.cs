using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITestButton : MonoBehaviour {

    public GameObject[] objArray;
    private int activateNext = 0;

    void Start()
    {
        objArray = GameObject.FindGameObjectsWithTag("UI");
        foreach (GameObject i in objArray)
        {
            i.SetActive(false);
        }
    }


    public void EnableObject ()
    {
        Debug.Log(activateNext);
        if (activateNext <= objArray.Length)
        {
               objArray[activateNext].SetActive(true);
                activateNext++;
        } else 
        {
            activateNext = 0;
            foreach (GameObject i in objArray)
            {
                i.SetActive(false);
            }

        }
    }
}
