using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f;

    [Header("Jump Settings")]
    public float jumpForce = 5f;
    public bool isGrounded;
    public LayerMask groundMask;

    private Rigidbody rb;
    private Vector3 moveDirection;

    void Start()
    {
        // Get Rigidbody component
        rb = GetComponent<Rigidbody>();

        // Freeze rotation to avoid tumbling
        rb.freezeRotation = true;
    }

    void Update()
    {
        // Basic Input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calculate direction
        moveDirection = new Vector3(horizontal, 0, vertical).normalized;

        // Rotate character towards the movement direction
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        // Move the character
        Vector3 movement = moveDirection * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the character is grounded
        if ((groundMask.value & (1 << collision.gameObject.layer)) > 0)
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Reset grounded status when leaving ground
        if ((groundMask.value & (1 << collision.gameObject.layer)) > 0)
        {
            isGrounded = false;
        }
    }
}
