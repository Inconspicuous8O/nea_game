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
         if(Input.GetMouseButton(1))
         {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            CameraMovements();
         }
         else
         {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

         }
        
    }

    void CameraMovements()
    {
        rotX += Input.GetAxis("Mouse X") * sensX;
        rotY += Input.GetAxis("Mouse Y") * sensY;

        rotY = Mathf.Clamp(rotY, minY, maxY);
        transform.rotation = Quaternion.Euler(-rotY, rotX,0);

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        float y = 0;

        if(Input.GetKey(KeyCode.Space))
        {
            y = 1;
        }
        else if (Input.GetKey(KeyCode.C))
        {
            y = -1;
        }

        Vector3 dir = transform.right * x + transform.up * y + transform.forward * z ;
        
        if(Input.GetKey(KeyCode.LeftShift))
        {
            currentMoveSpeed = sprintMoveSpeed;
        }
        else
        {
            currentMoveSpeed = normalMoveSpeed;

        }
        transform.position += dir * currentMoveSpeed * Time.deltaTime;
    }




}