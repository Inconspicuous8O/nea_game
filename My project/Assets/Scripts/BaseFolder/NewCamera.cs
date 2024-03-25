using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCamera : MonoBehaviour
{
    public float sensY;
    public float sensX;

    public float minY;
    public float maxY;

    private float rotX;
    private float rotY;

    public float normalMoveSpeed;
    public float sprintMoveSpeed;

    private float currentMoveSpeed;

    void LateUpdate ()
    {   
        /// Called if the right mouse button is held down
         if(Input.GetMouseButton(1))
         {
            Cursor.visible = false; ///Sets the cursor to invisible
            Cursor.lockState = CursorLockMode.Locked; /// Locks cursor to the middle of the screen
            CameraMovements(); /// Calls the camera movement function
         }
         else
         {
            Cursor.visible = true; ///Sets the cursor to visible
            Cursor.lockState = CursorLockMode.None; /// Unlocks cursor

         }
        
    }

    void CameraMovements()
    {
        rotX += Input.GetAxis("Mouse X") * sensX; /// Gets the horizontal mouse movement
        rotY += Input.GetAxis("Mouse Y") * sensY; /// Gets the vertical mouse movement

        rotY = Mathf.Clamp(rotY, minY, maxY); /// Makes it so that the vertical movement can't go beyond a certain limit
        transform.rotation = Quaternion.Euler(-rotY, rotX,0); /// Implements the mouse movements

        float x = Input.GetAxis("Horizontal"); /// Gets the strafe movement
        float z = Input.GetAxis("Vertical"); /// Gets the forward and backward movement
        float y = 0; /// No upward and downward movement

        if(Input.GetKey(KeyCode.Space))
        {
            y = 1; /// If spacebar is clicked, the camera will need to move up
        }
        else if (Input.GetKey(KeyCode.C))
        {
            y = -1; /// If c is clicked, the camera will need to move down
        }

        /// Creates a vector 3 that stores the value of the movement
        Vector3 dir = transform.right * x + transform.up * y + transform.forward * z ;
        
        if(Input.GetKey(KeyCode.LeftShift))
        {
            currentMoveSpeed = sprintMoveSpeed; ///Sets speed to move at to sprint speed when shift is pressed 
        }
        else
        {
            currentMoveSpeed = normalMoveSpeed;

        }
        transform.position += dir * currentMoveSpeed * Time.deltaTime; /// Implements the movement to the camera
    }




}