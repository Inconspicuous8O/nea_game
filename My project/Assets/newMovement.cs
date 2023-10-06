using System.Collections;
using System.Collections.Generic;
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
        if (grounded)
        {
            rb.drag = groundDrag;
            rb.mass = groundMass;
        }
        else
        {
            rb.drag = airDrag;
            rb.mass = airMass;
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(transform.up * jumpPower, ForceMode.Impulse);
            rb.AddForce(-(transform.up) * (forceDown), ForceMode.Impulse);
        }
    }

    void MovementControl()
    {
        float xmove = Input.GetAxisRaw("Horizontal");
        float zmove = Input.GetAxisRaw("Vertical");
        
        moveDirec = orientation.forward * zmove + orientation.right * xmove;


        //Speed Control//
        float totalVel = Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.y) + Mathf.Abs(rb.velocity.z);
        float placeholder = 0;
        if (totalVel > maxSpeed)
            placeholder = 0;
        else
            placeholder = 1;


        if (grounded)
            rb.AddForce(moveDirec.normalized * moveSpeed *10f * placeholder, ForceMode.Force);
        else
            rb.AddForce(moveDirec.normalized * moveSpeed *10f * airSpeed * placeholder, ForceMode.Force);
    }

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        rb.freezeRotation = true;
    }

    void Update()
    {   
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        MovementControl();

        Jump();

        GroundCheck();
    }

}

