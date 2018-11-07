using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ping : MonoBehaviour
{
    public bool scanner;
    public GameObject ping;
    public bool count;

    void Start()
    {

    }

    void Update ()
    {
        if (count)
        {
                for (int t = 0; t > 10; t++)
                {
                ping.gameObject.SetActive(false);
                }
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player" && this.gameObject != null)
        {
            scanner = true;
            ping.GetComponent<MeshRenderer>().enabled = true;
            StartCoroutine(Scale(10));

        } 
    }
    IEnumerator Scale(float time)
    {
        count = true;
        Vector3 originalScale = ping.transform.localScale;
        Vector3 destinationScale = new Vector3(500.0f, 500.0f);

        
        {
            float currentTime = 0.0f;

            do
            {

                ping.transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / time);
                currentTime += Time.deltaTime;
                yield return null;
            } while (currentTime <= time);
        }

        

    }
}

