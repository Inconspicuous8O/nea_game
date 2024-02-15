using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Change_canvas_script : MonoBehaviour
{
    public GameObject current_canvas;
    public GameObject new_canvas; 

    void Start()
    {
        Canvastracker.currentCanvasTracker = current_canvas;
    }

    public void Change_canvas()
    {
        current_canvas.SetActive(!current_canvas.activeSelf);
        new_canvas.SetActive(!new_canvas.activeSelf);
        Canvastracker.currentCanvasTracker = new_canvas;
    }
}
