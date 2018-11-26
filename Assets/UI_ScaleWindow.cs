using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ScaleWindow : MonoBehaviour
{

    void Start()
    {
        this.transform.localScale = new Vector3(Camera.main.orthographicSize * 2.0f * Screen.width / Screen.height, Camera.main.orthographicSize * 2.0f * Screen.width / Screen.height,1 );
    }

}
