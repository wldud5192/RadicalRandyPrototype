using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameUI : MonoBehaviour {

    public GameObject UI;
    float timer;
    bool startTimer;

    void Update ()

    {
        if (startTimer)
        {
            timer += Time.deltaTime;
        }

        if (timer > 5f && UI.active)
        {
            UI.SetActive(false);
        }
    }

    void OnTriggerEnter (Collider col)

    {
        if (col.gameObject.tag == "Player")
        {
            timer = 0;
            startTimer = true;
            UI.SetActive(true);
            
        }

    }
}
