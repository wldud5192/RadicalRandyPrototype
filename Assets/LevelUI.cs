using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelUI : MonoBehaviour {
    TMP_Text m_TextComponent;

    void Awake ()
    {
        m_TextComponent = GetComponent<TMP_Text>();
        Scene scene = SceneManager.GetActiveScene();
        m_TextComponent.text = "Level: " + scene.buildIndex;
    }

}
