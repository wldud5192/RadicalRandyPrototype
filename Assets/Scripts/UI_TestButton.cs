using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TestButton : MonoBehaviour
{

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


    public void EnableObject()
    {
        if (activateNext < objArray.Length)
        {
            objArray[activateNext].SetActive(true);
            activateNext++;
            objArray[activateNext].GetComponent<AudioSource>().Play();

        } if (activateNext == objArray.Length)
        {
            objArray[activateNext].SetActive(true);
            objArray[activateNext].GetComponent<AudioSource>().Play();
            activateNext = 0;
        }
    }
}
    