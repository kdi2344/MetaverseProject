using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroBtn : MonoBehaviour
{
    public void StartGame(){
        SceneManager.LoadScene("GameScene");
    }
    public void EndGame(){
         #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
