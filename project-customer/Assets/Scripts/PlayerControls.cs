using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControlType { Force, Position, Velocity }

public class SimplePhysicsControls : MonoBehaviour
{

    public ControlType control;

    public float moveForce;
    public float moveSpeed;
    public float SprintMultiplierForce;
    public float SprintMultiplierSpeed;
    public float jumpForce = 20;

    public float rayLength = 1;

    Rigidbody rb;
    bool grounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.sleepThreshold = 0;
    }

    private void Update()
    {
        if (IsGrounded() &&
            Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity += new Vector3(0, jumpForce, 0);
        }
    }

    bool IsGrounded()
    {

        RaycastHit hitInfo;
        //Debug.DrawRay(transform.position, Vector3.down * 1.1f,Color.yellow);
        //transform.parent = null;
        if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, 1.1f))
        {
            //GetComponent<ChangeColor>().SetColor(Color.blue);
            //if (hitInfo.collider.GetComponent<MovePlatform>() != null) { /*transform.parent = hitInfo.transform;*/ }
            return true;
        }
        return false;
    }
    private Vector3 GetCameraRelativeVector()
    {
        Vector2 playerInput = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));

        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight= Camera.main.transform.right;

        camForward.y = 0;
        camRight.y = 0;
        
        camForward = camForward.normalized;
        camRight= camRight.normalized;

        Vector3 relativeForward = playerInput.x * camForward;
        Vector3 relativeRight = playerInput.y * camRight;

        Vector3 relativePosition = relativeForward + relativeRight;

        return relativePosition;
    }

    void FixedUpdate()
    {
        //add func here
        Vector3 moveVector = GetCameraRelativeVector();
            //new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));


        //transform.Translate(moveVector * moveSpeed);

        if (IsGrounded())
        {
            control = ControlType.Velocity;
        }
        else
        {
            control = ControlType.Force;
        }

        switch (control)
        {
            case ControlType.Force:
                if(Input.GetButton("Sprint"))
                {
                    rb.AddForce(moveVector * moveForce * SprintMultiplierForce);
                }
                else
                {
                    rb.AddForce(moveVector * moveForce);
                }
                break;
            case ControlType.Velocity:
                Vector3 newVelocity = new Vector3(0, rb.velocity.y, 0);
                if (Input.GetButton("Sprint"))
                {
                    newVelocity += moveVector * moveSpeed * SprintMultiplierSpeed;
                }
                else
                {
                    newVelocity += moveVector * moveSpeed;
                }

                rb.velocity = newVelocity;
                break;
        }
        grounded = false;
    }

    private void OnCollisionStay(Collision collision)
    {

        //Debug.DrawRay(collision.GetContacts(0).point, collision.GetContacts(0).normal * rayLength, Color.magenta);

        if (collision.GetContact(0).normal.y > 0.8f)
        {
            //GetComponent<ChangeColor>().SetColor(Color.red);
            grounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //grounded = false;
    }
}

