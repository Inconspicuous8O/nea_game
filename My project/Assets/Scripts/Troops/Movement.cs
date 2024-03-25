using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Transform orientation;

    public float moveSpeed;
    public float airSpeed;
    public float jumpPower;
    public float maxSpeed;

    public Rigidbody rb;
    Vector3 moveDirec;

    public float groundDrag;
    public float groundMass;
    public float airDrag;
    public float airMass;
    public float forceDown;

    //gound check//
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    void GroundCheck()
    {
        if (grounded) /// sets drag and mass to ground values
        {
            rb.drag = groundDrag;
            rb.mass = groundMass;
        }
        else /// sets drag and mass to air values
        {
            rb.drag = airDrag;
            rb.mass = airMass;
        }
    }

    void Jump()
    {
        /// if space is pressed and object is on the ground, jump
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); /// reset vertical component
            rb.AddForce(transform.up * jumpPower, ForceMode.Impulse); /// apply vertical force
            rb.AddForce(-(transform.up) * (forceDown), ForceMode.Impulse); /// apply downwards force
        }
    }

    void MovementControl()
    {
        float xmove = Input.GetAxisRaw("Horizontal"); /// retrieve horizontal inputs
        float zmove = Input.GetAxisRaw("Vertical"); /// retrieve vertical inputs
        
        moveDirec = orientation.forward * zmove + orientation.right * xmove; /// turns player


        //Speed Control//
        float totalVel = Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.y) + Mathf.Abs(rb.velocity.z); /// calculates total velocity
        float placeholder = 0; /// define local variable
        
        if (totalVel > maxSpeed)
        {
            placeholder = 0; /// max speed reached so dont increase speed
        }
        else
        {
            placeholder = 1; /// max speed not reached so fine to increase speed
        }

        if (grounded)
        {
            rb.AddForce(moveDirec.normalized * moveSpeed *10f * placeholder, ForceMode.Force); /// Apply force to adjust speed when on land
        }
        else
        {
            rb.AddForce(moveDirec.normalized * moveSpeed *10f * airSpeed * placeholder, ForceMode.Force); /// Apply force to adjust speed when in air
        }
    }

    void Start()
    {
        CustomStart();
    }

    public void CustomStart()
    {
        Cursor.lockState = CursorLockMode.Locked; /// cursor is locked in the middle of the screen
        rb.freezeRotation = true;/// free rotation of object
    }

    void Update()
    {   
        /// Shoots raycast and check if character is on the ground or not
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        
        /// call required functions 
        MovementControl();
        Jump();
        GroundCheck();
    }

}

