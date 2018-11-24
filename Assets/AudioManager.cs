using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioClip normalBGM;
    public AudioClip alertedBGM;
    PlayerController playerCont;
    AudioSource audioSource;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
        playerCont = GameObject.Find("Player").GetComponent<PlayerController>();
        audioSource.clip = normalBGM;
    }
	
	// Update is called once per frame
	void Update () {
		if(playerCont.playerIsDetected)
        {
            if (!playerCont.played)
            {
                ChangeBGM(alertedBGM);
            }
            
        }
    }
    
    void ChangeBGM(AudioClip music)
    {
            audioSource.Stop();
            audioSource.clip = music;
            audioSource.Play();
    }    
}
