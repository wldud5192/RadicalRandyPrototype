using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticeBarUI : MonoBehaviour {

    Quaternion rotation;
     
    void Awake()
    {
        rotation = transform.rotation;
    }
    void Update()
    {
        transform.rotation = rotation;
    }
}
