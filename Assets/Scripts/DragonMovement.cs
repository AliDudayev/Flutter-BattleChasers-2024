using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonMovement : MonoBehaviour
{
    public float radius = 5.0f;  // Radius of the circle
    public float speed = 2.0f;   // Speed of rotation
    public Vector3 center = Vector3.zero; // Center point of the circle

    private Animator an;

    private Rigidbody rb;
    private float angle = 0.0f; // Current angle of rotation

    void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();
        an = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        // Update the angle based on the speed
        angle += speed * Time.fixedDeltaTime;

        // Calculate the new position
        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;

        // Determine the target position
        Vector3 targetPosition = center + new Vector3(x, 0, z);

        // Move the Rigidbody to the new position
        rb.MovePosition(targetPosition);

        // Rotate to face the direction of movement
        Vector3 direction = targetPosition - transform.position;
        if (direction != Vector3.zero) // Avoid errors when direction is zero
        {
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            rb.MoveRotation(toRotation);
        }
    }

}
