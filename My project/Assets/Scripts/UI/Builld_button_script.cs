using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class Builld_button_script : MonoBehaviour
{
    public GameObject current_canvas;
    public GameObject new_canvas; 

    public void Build_action()
    {
        Debug.Log("Build button was pressed");
        current_canvas.SetActive(!current_canvas.activeSelf);
        new_canvas.SetActive(!new_canvas.activeSelf);
    }
}
