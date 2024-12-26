using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Terresquall;

public class CharacterController : MonoBehaviour
{
    [Header("Character Settings")]
    [SerializeField] int strength = 100;

    [Header("Movement Settings")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotationSpeed = 720f;

    [Header("Animations")]
    private Animator animator;

    [Header("Attack")]
    [SerializeField] AttackCollider attackCollider;
    [SerializeField] ParticleSystem stabParticle;
    [SerializeField] ParticleSystem swordTrail;

    private Rigidbody rb;
    private Vector3 moveDirection;

    private bool isDead = false;  

    private float comboTimer = 0;
    private bool isAttacking = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true;

        animator = GetComponent<Animator>();

        attackCollider.SetPower(strength);
    }

    void Update()
    {
        if (isDead) return;

        if(comboTimer > 0)
        {
            comboTimer -= Time.deltaTime;
        }
        else if(comboTimer <= 0 && isAttacking)
        {
            isAttacking = false;
            animator.SetBool("Attacking", false);
        }

        // Dit is basic Input
        //float horizontal = Input.GetAxis("Horizontal");
        //float vertical = Input.GetAxis("Vertical");
        float horizontal = VirtualJoystick.GetAxis("Horizontal");
        float vertical = VirtualJoystick.GetAxis("Vertical");

        //// Dit calculate de movement richting
        //moveDirection = new Vector3(horizontal, 0, vertical);
        // Get the Vuforia camera's forward and right directions
        Transform cameraTransform = Camera.main.transform;
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        // Flatten the forward and right vectors to ignore vertical tilt
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        // Calculate the movement direction relative to the camera
        moveDirection = camForward * vertical + camRight * horizontal;

        // Dit bestuurt de loop animatie
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

        // Linkermuisknop om aan te vallen
        //if (Input.GetButtonDown("Fire1"))
        //{
        //    Attack();
        //}
    }

    public void Attack()
    {
        animator.SetBool("Attacking", true);
        comboTimer = 0.4f;
    }

    private void SetAttack(int playerIsAttacking)
    {
        if(playerIsAttacking == 1)
        {
            isAttacking = true;
            attackCollider.SetIsAttacking(true);

            attackCollider.SetSize(new Vector3(0.8390284f, 2.612277f, 0.2480217f));
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

    private void SetStrengthAttack(float extraAttack)
    {
        int calculatedPower = Mathf.RoundToInt((float)(strength * extraAttack));
        attackCollider.SetPower(calculatedPower);
    }

    private void PlayParticle(int particleToPlay)
    {
        swordTrail.Play();
        StartCoroutine(DeactivateSwordTrail());

        if (particleToPlay == 1)
        {
            attackCollider.SetSize(new Vector3(0.8390284f, 5f, 0.2480217f));
            stabParticle.Play();
        }
    }

    private IEnumerator DeactivateSwordTrail()
    {
        yield return new WaitForSeconds(1f);
        swordTrail.Stop();
    }

    private void StopParticle()
    {
        stabParticle.Stop();
    }

    void FixedUpdate()
    {
        if (isDead) return;

        // Beweeg de character
        Vector3 movement = moveDirection * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
        if (movement != Vector3.zero)
        {
            rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
        }
    }

    //public void SetIsDead(bool setIsDead)
    //{
    //    isDead = setIsDead;
    //    if(isDead == false)
    //    {
    //        isAttacking = false;
    //    }
    //}
}
