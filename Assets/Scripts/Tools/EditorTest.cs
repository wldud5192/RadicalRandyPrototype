#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.SceneManagement;

public class EditorTest : EditorWindow
{
    bool isCreated = false;

    [MenuItem("Tools/Scene From Scratch")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(EditorTest));
    }

    void OnGUI()
    {
        if (!isCreated)
        {

            EditorApplication.NewScene();
            /*GameObject c = GameObject.Find("Main Camera");
            if (c != null)
                DestroyImmediate(c);

    */

            GameObject g = new GameObject("Scene From Scratch");
            g.AddComponent<SceneFromScratch>(); // a scene generating script
            
            

            EditorWindow.GetWindow(typeof(EditorTest)).Close();
            isCreated = true;
        }
    }
}
#endif