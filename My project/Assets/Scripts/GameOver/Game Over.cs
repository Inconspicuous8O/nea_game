using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOver : MonoBehaviour
{
    public static void ChangeScene()
    {
        Points.SaveScore(); /// saving score using unity
        SceneManager.LoadScene(1); /// load death scene
    }
}
