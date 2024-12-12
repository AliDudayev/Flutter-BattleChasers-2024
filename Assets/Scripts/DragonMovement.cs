using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonMovement : MonoBehaviour
{
    public float circleRadius = 5.0f;    // Radius for circling
    public float circleSpeed = 2.0f;    // Speed of circling
    public Vector3 circleCenter = Vector3.zero; // Center of circling movement

    public float flyToPlayerSpeed = 5.0f;   // Speed when flying directly to the player
    public float landDistance = 10.0f;      // Distance to land away from the player
    public float descentSpeed = 1.0f;       // Speed of descent during combat setup

    public Transform player;               // Player reference

    private Animator animator;             // Animator for the dragon
    private Rigidbody rb;                  // Rigidbody for movement
    private float angle = 0.0f;            // Angle for circular motion
    private float landHeight;              // Ground level for landing

    private string behavior = "FlyInCircles"; // Initial behavior
    private bool transitioningToPlayer = false;

    private Vector3 landingPosition;       // Landing position variable

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        landHeight = player.position.y; // Assume player's Y as the ground level

        // Start circling behavior
        StartCoroutine(TransitionToFlyToPlayer());
    }

    void FixedUpdate()
    {
        switch (behavior)
        {
            case "FlyInCircles":
                FlyInCircles();
                break;
            case "FlyToPlayer":
                FlyToPlayer();
                break;
            case "Combat":
                Combat();
                break;
            default:
                break;
        }
    }

    private void FlyInCircles()
    {
        // Circling around the center
        angle += circleSpeed * Time.fixedDeltaTime;

        // Calculate new position on the circle
        float x = Mathf.Cos(angle) * circleRadius;
        float z = Mathf.Sin(angle) * circleRadius;

        // Determine target position for circling
        Vector3 targetPosition = circleCenter + new Vector3(x, 0, z);

        // Move the Rigidbody to the target position
        rb.MovePosition(targetPosition);

        // Rotate to face the direction of movement
        Vector3 direction = targetPosition - transform.position;
        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            rb.MoveRotation(toRotation);
        }
    }

    private void FlyToPlayer()
    {
        // Fly straight toward the player
        Vector3 targetPosition = new Vector3(player.position.x, player.position.y, player.position.z);

        // Move towards the player's position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, flyToPlayerSpeed * Time.fixedDeltaTime);

        // Rotate to face the player
        Vector3 directionToPlayer = (targetPosition - transform.position).normalized;
        Quaternion toRotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.fixedDeltaTime * 2f);
        Debug.Log("FlyToPlayer behavior");

        // Transition to Combat behavior when close enough
        if (Vector3.Distance(transform.position, targetPosition) < landDistance)
        {
            Debug.Log("Combat Transition");
            animator.SetTrigger("Land");
            behavior = "Combat";
        }
    }

    private void Combat()
    {
        Debug.Log("Combat behavior");
        // If we haven't already calculated the landing position, do it now
        if (landingPosition == Vector3.zero)
        {
            // Calculate safe landing position near the player
            Vector3 directionToPlayer = (transform.position - player.position).normalized;
            landingPosition = player.position + directionToPlayer * landDistance;
            landingPosition.y = landHeight;  // Ensure we land at the correct height
        }

        // Descend to the landing position
        transform.position = Vector3.MoveTowards(transform.position, landingPosition, descentSpeed * Time.fixedDeltaTime);

        // Rotate to face the player once landed
        if (Vector3.Distance(transform.position, landingPosition) < 0.5f)
        {
            Quaternion toRotation = Quaternion.LookRotation(player.position - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.fixedDeltaTime * 2f);

            // Trigger combat-ready animations or other logic
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Combat"))
            {
                animator.SetTrigger("Combat");
            }
        }
    }

    private IEnumerator TransitionToFlyToPlayer()
    {
        // Wait for 5 seconds while circling
        yield return new WaitForSeconds(5f);

        // Transition to flying directly toward the player
        behavior = "FlyToPlayer";
    }
}
