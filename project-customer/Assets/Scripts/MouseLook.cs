using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public enum Direction
    {
        None,
        Horizontal = (1 << 0),
        Vertical = (1 << 1)
    }
    [SerializeField] private Direction directions;
    [SerializeField] private Vector2 acceleration;
    [SerializeField] public Vector2 sensitivity;
     private Vector2 velocity;
     private Vector2 rotation;

    [SerializeField] private float maxVerticalAngle;

    private Vector2 lastInput;
    [SerializeField] private float inputLagPeriod;
    private float inputLagTimer;
    public bool CanLookAround;

    private void OnEnable()
    {
        CanLookAround = true;
        velocity = Vector3.zero;
        inputLagTimer = 0;
        lastInput = Vector3.zero;

        Vector3 euler = transform.localEulerAngles;

        if (euler.x >= 180)
        {
            euler.x -= 360;
        }
        euler.x = Mathf.Clamp(euler.x, -maxVerticalAngle, maxVerticalAngle);

        rotation= new Vector2(euler.x, euler.y);
    }

    private Vector2 GetInput()
    {
        inputLagTimer += Time.deltaTime;

        Vector2 input = new Vector2(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y")
            );

        if ((Mathf.Approximately(0, input.x) && Mathf.Approximately(0, input.y)) == false || 
            inputLagTimer >= inputLagPeriod) 
        {
            lastInput = input;
            inputLagTimer = 0;
        }

        return lastInput;
    }

    private void Update()
    {
        if (!CanLookAround) return;

        Vector2 appliedVelocity = GetInput() * sensitivity;

        if((directions & Direction.Horizontal) == 0)
        {
            appliedVelocity.x = 0;
        }
        if((directions & Direction.Vertical) == 0)
        {
            appliedVelocity.y = 0;
        }

        velocity = new Vector2(
            Mathf.MoveTowards(velocity.x, appliedVelocity.x, acceleration.x * Time.deltaTime),
            Mathf.MoveTowards(velocity.y, appliedVelocity.y, acceleration.y * Time.deltaTime));
        
        rotation += velocity * Time.deltaTime;
        rotation.y= Mathf.Clamp(rotation.y,-maxVerticalAngle, maxVerticalAngle);
        
        transform.localEulerAngles = new Vector3(rotation.y, rotation.x, 0);
        }
}

