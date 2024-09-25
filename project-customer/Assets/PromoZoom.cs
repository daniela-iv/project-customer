using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromoZoom : MonoBehaviour
{
    public Transform target;  // The target to zoom in on (e.g., family portrait)
    public float stopDistance = 1f;  // How close the camera should get to the target
    public float zoomDuration = 38f;  // How long it should take to reach the target (in seconds)

    private Vector3 initialPosition;  // Store the camera's starting position
    private float elapsedTime = 0f;  // Track how much time has passed
    private float totalDistance;  // Total distance from initial position to the target
    private Vector3 targetAdjustedPosition;  // Position adjusted by stop distance

    void Start()
    {
        initialPosition = transform.position;  // Save the initial position of the camera

        // Calculate the total distance between the camera and the adjusted target position (by stopDistance)
        totalDistance = Vector3.Distance(initialPosition, target.position) - stopDistance;

        // Calculate the position at which we stop, by moving back the stopDistance from the target
        targetAdjustedPosition = target.position - (target.position - initialPosition).normalized * stopDistance;
    }

    void Update()
    {
        // Only zoom while the elapsed time is less than the zoom duration
        if (elapsedTime < zoomDuration)
        {
            elapsedTime += Time.deltaTime;  // Increase the elapsed time

            // Calculate the progress based on the elapsed time and duration
            float progress = Mathf.Clamp01(elapsedTime / zoomDuration);

            // Smoothly interpolate between the initial position and the adjusted stop position
            transform.position = Vector3.Lerp(initialPosition, targetAdjustedPosition, Mathf.SmoothStep(0f, 1f, progress));
        }
    }
}