using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ping : MonoBehaviour
{
    public GameObject ping;


    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            ping.GetComponent<MeshRenderer> ().enabled = true;

            
        }
    }
}
