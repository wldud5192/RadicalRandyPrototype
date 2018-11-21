using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthScript : MonoBehaviour
{
    AudioSource deathAudio;

    public int maxLife = 5;
    public int curLife;
    bool isDead = false;

    GameObject gameOverUI;
    public Image[] lifeImages;
    GameObject[] lifeUI;
    public Sprite fullLife;
    public Sprite emptyLife;

    void Start()
    {
        deathAudio = GetComponent<AudioSource>();
        DontDestroyOnLoad(this.gameObject);
        gameOverUI = GameObject.Find("GameOverUI");
        lifeUI = GameObject.FindGameObjectsWithTag("LifeUI");

        for (int i = 0; i < maxLife; i++)
        {
            lifeImages[i] = lifeUI[i].GetComponent<Image>();
        }

        curLife = maxLife;
    }

    void Update()
    {
        if (curLife < 0)
        {
            if (!isDead)
            {
                StartCoroutine(Death());
            }
        }

        if (maxLife < curLife)
        {
            maxLife = curLife;
        }

        for (int i = 0; i < lifeImages.Length; i++)
        {
            if (i < maxLife)
            {
                lifeImages[i].sprite = fullLife;
            }
            else
            {
                lifeImages[i].sprite = emptyLife;
            }
            if (i < curLife)
            {
                lifeImages[i].enabled = true;
            }
            else
            {
                lifeImages[i].enabled = false;
            }
        }
    }


    IEnumerator Death()
    {
        deathAudio.Play();
        yield return new WaitForSeconds(deathAudio.clip.length);
        SceneManager.LoadScene("GameOver");       
        isDead = true;
        Destroy(this.gameObject);
    }

    void CheckLifeAmount()
    {
        for (int i = 0; i < maxLife; i++)
        {
            if (curLife <= i)
            {
                lifeImages[i].enabled = false;
            }
            else
            {
                lifeImages[i].enabled = true;
            }

        }
    }

    void OnPlayerKilled()
    {
        if (curLife - 1 > 0)
        {
            curLife -= 1;
        }

        else

        {

            gameOverUI.gameObject.SetActive(true);

        }

    }

}
