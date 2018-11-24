using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticeBarUI : MonoBehaviour {

    AudioSource audio;
    Slider mainSlider;
    Quaternion rotation;
    float t = 0.1f;

    void Start()
    {
        audio =  GetComponent<AudioSource>();
        rotation = transform.rotation;
        mainSlider = GetComponent<Slider>();
        mainSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }
    void Update()
    {
        transform.rotation = rotation;
    }

    public void ValueChangeCheck()
    {
        t -= Time.deltaTime;
        if (t < 0)
        {
            audio.Play();
            t = 0.1f;
        }
    }
}
