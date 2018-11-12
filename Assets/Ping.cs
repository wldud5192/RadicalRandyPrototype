using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ping : MonoBehaviour
{
    public bool scanner;
    public GameObject ping;


    void Start()
    {

    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player" && ping.gameObject != null)
        {
            scanner = true;
            ping.GetComponent<MeshRenderer>().enabled = true;
            StartCoroutine(Scale(10));

        } 
    }
    IEnumerator Scale(float time)
    {

        Vector3 originalScale = ping.transform.localScale;
        Vector3 destinationScale = new Vector3(0.50f, 0.50f);

        
        {
            float currentTime = 0.0f;

            do
            {

                ping.transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / time);
                currentTime += Time.deltaTime;
                yield return null;
            } while (currentTime <= time);
        }

        Destroy(this.gameObject);
        

    }
}

