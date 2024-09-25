using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortraitSwingAndFall : MonoBehaviour
{
    private Rigidbody rb;
    private HingeJoint hingeJoint;

    public float initialWaitTime = 2f;  // Time to wait before the swing starts
    public float swingDuration = 2f;  // Time to let the frame swing before removing the hinge and letting it fall
    public float dampingFactor = 0.5f;  // Factor to apply the damping force

    private bool isSwinging = false;
    private bool isFalling = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        hingeJoint = GetComponent<HingeJoint>();

        // Disable gravity initially so the frame doesn't fall
        rb.useGravity = false;

        // Wait for the initial delay, then start the swing
        Invoke("StartSwing", initialWaitTime);
    }

    void StartSwing()
    {
        // Enable gravity to let the frame swing naturally
        rb.useGravity = true;

        // The frame is now swinging
        isSwinging = true;

        // Set the timer to remove the hinge and let the frame fall
        Invoke("TriggerFall", swingDuration);
    }

    void FixedUpdate()
    {
        // Apply a damping force to slow down the swing
        if (isSwinging && !isFalling)
        {
            float dampingForce = rb.angularVelocity.z * dampingFactor;  // Calculate damping force based on angular velocity
            rb.AddTorque(-dampingForce * Vector3.forward);  // Apply the damping force in the opposite direction to the swing
        }
    }

    void TriggerFall()
    {
        if (!isFalling)
        {
            // Remove the hinge joint to let the frame fall freely
            Destroy(hingeJoint);

            // Now the frame is falling
            isFalling = true;
        }
    }
}
