using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    [Header("Character Settings")]
    [SerializeField] int strength = 100;
    //[SerializeField] float maxHealth = 1000f;
    //private float health;
    //private Slider slider;
    //private float stamina = 90f;

    [Header("Movement Settings")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotationSpeed = 720f;

    //[Header("Jump Settings")]
    //[SerializeField] float jumpForce = 5f;
    //[SerializeField] bool isGrounded;
    //[SerializeField] LayerMask groundMask;

    [Header("Animations")]
    private Animator animator;

    [Header("Attack")]
    [SerializeField] AttackCollider attackCollider;

    private Rigidbody rb;
    private Vector3 moveDirection;

    //private bool isAttacking = false;  
    private bool isDead = false;  

    private float comboTimer = 0;
    //private float damageTimer = 0;
    private bool isAttacking = false;

    void Start()
    {
        // Get Rigidbody component
        rb = GetComponent<Rigidbody>();

        // Freeze rotation to avoid tumbling
        rb.freezeRotation = true;

        animator = GetComponent<Animator>();

        attackCollider.SetPower((int)strength);

        //health = maxHealth;
        //slider = GetComponentInChildren<Slider>();
        //slider.maxValue = maxHealth;
        //slider.value = health;
    }

    void Update()
    {
        if (isDead) return;

        //if(damageTimer > 0)
        //{
        //    damageTimer -= Time.deltaTime;
        //    //if(damageTimer <= 0)
        //    //{
        //    //    attackCollider.SetIsAttacking(false);
        //    //}
        //}
        if(comboTimer > 0)
        {
            comboTimer -= Time.deltaTime;
            //animator.SetTrigger("Attack");
            //Debug.Log("Combo");
        }
        else if(comboTimer <= 0 && isAttacking)
        {
            isAttacking = false;
            //attackCollider.SetIsAttacking(false);
            animator.SetBool("Attacking", false);
        }

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
        if (Input.GetButtonDown("Fire1"))
        {
            //StartCoroutine(Attack());  // Start attack coroutine to prevent rapid spamming

            //Attack();

            if(isAttacking == false)
            {
                //damageTimer = 1.2f;
                //animator.SetTrigger("Attack");
                //attackCollider.SetIsAttacking(true);
                animator.SetBool("Attacking", true);
            }
            else
            {
                comboTimer = 0.5f;
            }
        }

        // Handle death (just for demonstration, e.g. pressing a key)
        //if (Input.GetButtonDown("Fire2") && !isDead)
        //{
        //    ApplyDamage(200);  // Trigger death animation when pressing another key
        //}
    }

    private void SetAttack(int playerIsAttacking)
    {
        if(playerIsAttacking == 1)
        {
            isAttacking = true;
            attackCollider.SetIsAttacking(true);

        }
        else
        {
            if(comboTimer <= 0)
            {
                isAttacking = false;
                attackCollider.SetIsAttacking(false);
                animator.SetBool("Attacking", false);
            }
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

    //void OnCollisionEnter(Collision collision)
    //{
    //    // Check if the character is grounded
    //    if ((groundMask.value & (1 << collision.gameObject.layer)) > 0)
    //    {
    //        isGrounded = true;
    //        //animator.SetBool("isGrounded", true);  // Set grounded status in the animator
    //    }
    //}

    //void OnCollisionExit(Collision collision)
    //{
    //    // Reset grounded status when leaving ground
    //    if ((groundMask.value & (1 << collision.gameObject.layer)) > 0)
    //    {
    //        isGrounded = false;
    //        //animator.SetBool("isGrounded", false);  // Set grounded status in the animator
    //    }
    //}

    // Attack Coroutine to handle attack animation timing
    //private IEnumerator Attack()
    //{
    //    isAttacking = true;
    //    animator.SetTrigger("Attack");  // Trigger attack animation
    //    yield return new WaitForSeconds(1f);  // Assuming the attack animation takes 1 second
    //    isAttacking = false;
    //}
    //private void Attack()
    //{
    //    isAttacking = true;
    //    animator.SetTrigger("Attack");
    //    damageTimer += 1f;
    //}

    // Death Coroutine to handle death animation

    //public void ApplyDamage(float damage)
    //{
    //    health -= damage;
    //    slider.value = health;
    //    if (health <= 0 && !isDead)
    //    {
    //        StartCoroutine(Die());  // Trigger death animation when pressing another key
    //    }
    //}

    //private IEnumerator Die()
    //{
    //    isDead = true;
    //    animator.SetTrigger("Death");  // Trigger death animation
    //    yield return new WaitForSeconds(2f);  // Wait for death animation to finish (assuming 2 seconds)
    //    // Optionally, disable the character or handle further game logic here
    //}

    //public void Attack(float staminaCost)
    //{
    //    //stamina -= staminaCost;
    //    ApplyDamage(strength * ((staminaCost / 10) * 1.1f));
    //    Debug.Log(strength * ((staminaCost / 10) * 1.1f));
    //}

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
