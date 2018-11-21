using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelUI : MonoBehaviour
{
    TMP_Text m_TextComponent;

    void Start()
    {
        m_TextComponent = GetComponent<TMP_Text>();
        InvokeRepeating("CheckCurrentLevel", 0.5f, 1.0f);
    }

    void CheckCurrentLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        m_TextComponent.text = "Level: " + scene.buildIndex;
        if (scene.name == "GameOver")
        {
            Destroy(Camera.main);
        }
    }

}
