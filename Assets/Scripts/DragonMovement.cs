using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonMovement : MonoBehaviour
{
    public float circleRadius = 5.0f;    // Radius for circling
    public float circleSpeed = 2.0f;    // Speed of circling
    public Vector3 circleCenter;        // Center of circling movement (set dynamically)

    public float flyToPlayerSpeed = 5.0f;   // Speed when flying directly to the player
    public float landDistance = 10.0f;      // Distance to land away from the player
    public float descentSpeed = 1.0f;       // Speed of descent during combat setup

    public Transform player;               // Player reference

    private Animator animator;             // Animator for the dragon
    private Rigidbody rb;                  // Rigidbody for movement
    private float angle = 0.0f;            // Angle for circular motion
    private float groundHeight;            // Ground level for landing

    private string behavior = "FlyInCircles"; // Initial behavior
    private Vector3 landingPosition;       // Landing position variable

    private float walkSpeed = 2.0f;          // Speed for walking during combat
    private float attackRange = 2.0f;        // Range within which the dragon attacks the player

    private float attackCooldown = 4.0f;    // Time between attacks
    private float attackTimer = 0.0f;       // Timer to track cooldown

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        // Set the initial circling center to the dragon's spawn position
        circleCenter = transform.position;

        // Store player's ground height (assume they are on the ground initially)
        groundHeight = player.position.y;

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
            case "Land":
                Land();
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
        // Fly toward the player, including adjusting height
        Vector3 targetPosition = player.position;

        // Move toward the player's position (including y-axis)
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, flyToPlayerSpeed * Time.fixedDeltaTime);

        // Rotate to face the player
        Vector3 directionToPlayer = (targetPosition - transform.position).normalized;
        Quaternion toRotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.fixedDeltaTime * 2f);

        // Transition to Combat behavior when within landDistance
        if (Vector3.Distance(transform.position, targetPosition) < landDistance)
        {
            animator.SetTrigger("Land");
            behavior = "Land";
        }
    }

    private void Land()
    {
        // Calculate landing position if not already set
        if (landingPosition == Vector3.zero)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            landingPosition = player.position - directionToPlayer * landDistance;
            landingPosition.y = groundHeight; // Ensure correct landing height
        }

        // Descend and move toward the landing position
        transform.position = Vector3.MoveTowards(transform.position, landingPosition, descentSpeed * Time.fixedDeltaTime);

        // Check if landed
        if (Vector3.Distance(transform.position, landingPosition) < 0.5f)
        {
            // Rotate to face the player
            Quaternion toRotation = Quaternion.LookRotation(player.position - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.fixedDeltaTime * 2f);

            // Trigger combat-ready animations or other logic
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Combat"))
            {
                //animator.SetTrigger("Combat");
                //StartCoroutine(ChangeToCombat());
            }
        }
    }

    //private IEnumerator ChangeToCombat()
    //{
    //    yield return new WaitForSeconds(5.5f);
    //    behavior = "Combat";
    //}

    public void ChangeBehaviour(string behaviour)
    {
        this.behavior = behaviour;
    }

    private void Combat()
    {
        // Calculate the distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Update the attack timer
        attackTimer -= Time.deltaTime;

        if (distanceToPlayer > attackRange)
        {
            // Move toward the player
            animator.SetBool("Running", true);

            // Adjust target position to ensure the dragon remains on the ground
            Vector3 targetPosition = new Vector3(player.position.x, groundHeight, player.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, walkSpeed * Time.fixedDeltaTime);

            // Rotate to face the player
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            Quaternion toRotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.fixedDeltaTime * 2f);
        }
        else
        {
            // Stop moving
            animator.SetBool("Running", false);

            // Attack if the cooldown is complete
            if (attackTimer <= 0)
            {
                animator.SetTrigger("Attacking");
                attackTimer = attackCooldown; // Reset the cooldown
            }
        }
    }


    private IEnumerator TransitionToFlyToPlayer()
    {
        // Wait for 5 seconds while circling
        yield return new WaitForSeconds(5f);

        // Transition to flying directly toward the player
        behavior = "FlyToPlayer";
        animator.SetTrigger("Glide");
    }
}
