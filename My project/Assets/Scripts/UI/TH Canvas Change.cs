using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class THCanvasChange : MonoBehaviour
{
    private GameObject current_canvas;
    public GameObject new_canvas; 

    public void TH_Change_canvas()
    {
        /// change current canvas with value in canvas tracker script
        current_canvas = Canvastracker.currentCanvasTracker;
        ///changes if canvases are on or off
        current_canvas.SetActive(!current_canvas.activeSelf);
        new_canvas.SetActive(!new_canvas.activeSelf);
        /// set current canvas as value in canvas tracker
        Canvastracker.currentCanvasTracker = new_canvas;
    }
}
