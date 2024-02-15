using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class THCanvasChange : MonoBehaviour
{
    private GameObject current_canvas;
    public GameObject new_canvas; 

    public void TH_Change_canvas()
    {
        current_canvas = Canvastracker.currentCanvasTracker;
        current_canvas.SetActive(!current_canvas.activeSelf);
        new_canvas.SetActive(!new_canvas.activeSelf);
        Canvastracker.currentCanvasTracker = new_canvas;
    }
}
