using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    [Header("Character Settings")]
    public float strength = 100f;
    public float maxHealth = 1000f;
    private float health;
    private Slider slider;
    //private float stamina = 90f;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f;

    [Header("Jump Settings")]
    public float jumpForce = 5f;
    public bool isGrounded;
    public LayerMask groundMask;

    public Animator animator;

    private Rigidbody rb;
    private Vector3 moveDirection;

    private bool isAttacking = false;  // To track if the character is attacking
    private bool isDead = false;  // To track if the character is dead

    void Start()
    {
        // Get Rigidbody component
        rb = GetComponent<Rigidbody>();

        // Freeze rotation to avoid tumbling
        rb.freezeRotation = true;

        animator = GetComponent<Animator>();

        health = maxHealth;
        slider = GetComponentInChildren<Slider>();
        slider.maxValue = maxHealth;
        slider.value = health;
    }

    void Update()
    {
        if (isDead) return;

        // Basic Input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calculate direction
        moveDirection = new Vector3(horizontal, 0, vertical).normalized;

        // Handle walking animation
        if (moveDirection.magnitude > 0.1f)
        {
            animator.SetBool("Running", true);
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("Running", false);
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }

        // Jumping
        //if (Input.GetButtonDown("Jump") && isGrounded)
        //{
        //    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        //    animator.SetTrigger("jump");  // Trigger jump animation
        //}

        // Attack
        if (Input.GetButtonDown("Fire1") && !isAttacking)
        {
            StartCoroutine(Attack());  // Start attack coroutine to prevent rapid spamming
        }

        // Handle death (just for demonstration, e.g. pressing a key)
        if (Input.GetButtonDown("Fire2") && !isDead)
        {
            ApplyDamage(200);  // Trigger death animation when pressing another key
        }
    }

    void FixedUpdate()
    {
        if (isDead) return;

        // Move the character
        Vector3 movement = moveDirection * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
        if(movement != Vector3.zero)
        {
            rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the character is grounded
        if ((groundMask.value & (1 << collision.gameObject.layer)) > 0)
        {
            isGrounded = true;
            //animator.SetBool("isGrounded", true);  // Set grounded status in the animator
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Reset grounded status when leaving ground
        if ((groundMask.value & (1 << collision.gameObject.layer)) > 0)
        {
            isGrounded = false;
            //animator.SetBool("isGrounded", false);  // Set grounded status in the animator
        }
    }

    // Attack Coroutine to handle attack animation timing
    private IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");  // Trigger attack animation
        yield return new WaitForSeconds(1f);  // Assuming the attack animation takes 1 second
        isAttacking = false;
    }

    // Death Coroutine to handle death animation
    private IEnumerator Die()
    {
        isDead = true;
        animator.SetTrigger("Death");  // Trigger death animation
        yield return new WaitForSeconds(2f);  // Wait for death animation to finish (assuming 2 seconds)
        // Optionally, disable the character or handle further game logic here
    }

    public void ApplyDamage(float damage)
    {
        health -= damage;
        slider.value = health;
        if (health <= 0 && !isDead)
        {
            StartCoroutine(Die());  // Trigger death animation when pressing another key
        }
    }

    public void Attack(float staminaCost)
    {
        //stamina -= staminaCost;
        ApplyDamage(strength * ((staminaCost / 10) * 1.1f));
        Debug.Log(strength * ((staminaCost / 10) * 1.1f));
    }

    //public void Guard()
    //{
    //    Debug.Log("Guard");
    //}

    //public void GainStamina(float value)
    //{
    //    stamina += value;
    //    if (stamina > 90)
    //    {
    //        stamina = 90;
    //    }
    //}

    //public float GetStamina()
    //{
    //    return stamina;
    //}

}
