using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour
{
    public void Start()
    {
        Cursor.visible = true; /// turns cursor visible
        Cursor.lockState = CursorLockMode.None; /// unlock cursor from centre of screen
    }
    
    public static void RestartFunc()
    {
        SceneManager.LoadScene(0); /// load original game scene
    }
}
