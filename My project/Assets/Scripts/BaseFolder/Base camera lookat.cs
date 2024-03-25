using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basecameralookat : MonoBehaviour
{
    public float sensitivity;
    public float normalSpeed, sprintSpeed, downSens, upSens;
    float currentSpeed;
    
    void Update () {
         if(Input.GetMouseButton(1))
         {
            /// Cursor set to invisible and locked in the middle of the screen
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            /// Movement, rotation and vertical movement functions that are called
            Movement();
            Rotation();
            VerticalMove();

         }
         else
         {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

         }
    }

    void Rotation()
    {
        Vector3 mouseInput = new Vector3(-Input.GetAxis("Mouse Y"),Input.GetAxis("Mouse X"),0);
        transform.Rotate(mouseInput * sensitivity * Time.deltaTime * 50);
        Vector3 eulerRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(eulerRotation.x,eulerRotation.y,0);

    }
     
     void Movement()
     {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"),0f, Input.GetAxis("Vertical"));
        if(Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("shift key was pressed");
            currentSpeed = sprintSpeed;
        }
        else{
            currentSpeed = normalSpeed;
        }

        transform.Translate(input * currentSpeed * Time.deltaTime);
     }

     void VerticalMove()
     {  
        Vector3 input2 = new Vector3(0f,0f,0f);
        if(Input.GetKey("c"))
        {
            Debug.Log("c key was pressed");
            input2 = new Vector3(0f,-(50*downSens),0f);
        }
        else if(Input.GetKey(KeyCode.Space))
        {
            Debug.Log("space key was pressed");
            input2 = new Vector3(0f,(50*upSens),0f);
        }

        transform.Translate(input2 * Time.deltaTime);
     }

}
