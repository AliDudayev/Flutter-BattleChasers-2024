using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //public enum EnemyType { Melee, Ranged }
    //public EnemyType enemyType;

    [Header("Settings")]
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float attackRange = 2f;
    [SerializeField] int power = 50;
    //[SerializeField] private float rangedAttackRange = 10f; // Ranged attack distance

    [Header("Attack")]
    [SerializeField] private float attackCooldown = 1.5f; // Time between attacks
    private float attackTimer;
    [Header("Attack for melee enemy")]
    [SerializeField] AttackCollider attackCollider;
    [Header("Attack for ranged enemy")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] GameObject projectileSpawnLocation;
    [SerializeField] private float projectileSpeed = 10f;

    [Header("References")]
    private Transform playerTransform;
    private Animator animator;

    //public float heightProjectile;

    void Start()
    {
        animator = GetComponent<Animator>();
        attackTimer = attackCooldown;

        if (playerTransform == null)
        {
            playerTransform = GameObject.FindAnyObjectByType<CharacterController>().transform;
        }

        if(attackCollider != null) {
            attackCollider.SetPower(power);
        }
    }

        void Update()
    {
        if (playerTransform == null) return;

        LookAtPlayer(); // Ensure the enemy always faces the player

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        attackTimer -= Time.deltaTime;

        if (distanceToPlayer <= attackRange && attackTimer <= 0)
        {
            Attack();
            attackTimer = attackCooldown;
        }

        if (distanceToPlayer <= attackRange - 2) // If too close
        {
            MoveAwayFromPlayer();
        }
        else if (distanceToPlayer > attackRange)
        {
            MoveTowardsPlayer();
        }
        else
        {
            animator.SetBool("Running", false);
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
        //transform.LookAt(new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z));
        animator.SetBool("Running", true);
    }
    private void MoveAwayFromPlayer()
    {
        Vector3 direction = (transform.position - playerTransform.position).normalized; // Move away from player
        transform.position += direction * (moveSpeed / 2) * Time.deltaTime; // Half the move speed
        animator.SetBool("Running", true); // Play running animation
    }

    private void LookAtPlayer()
    {
        // Ensure the enemy is always facing the player
        Vector3 lookDirection = (playerTransform.position - transform.position).normalized;
        lookDirection.y = 0; // Lock Y-axis rotation to keep the enemy upright
        if (lookDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }
    }

    private void Attack()
    {
        animator.SetTrigger("Attacking");
        animator.SetBool("Running", false);
        //if (enemyType == EnemyType.Melee)
        //{
        //    Debug.Log("Melee Attack");
        //}
        //else if (enemyType == EnemyType.Ranged)
        //{
        //    Debug.Log("Ranged Attack");
        //    ShootProjectile();
        //}
        //ShootProjectile();
    }

    private void SetAbleToHit(int ableToHit)
    {
        if(0 == ableToHit)
        {
            attackCollider.SetIsAttacking(false);
        }
        else
        {
            attackCollider.SetIsAttacking(true);
        }
    }

    private void ShootProjectile()
    {
        if (projectilePrefab == null || playerTransform == null) return;

        // Set the spawn position slightly above the enemy
        //Vector3 spawnPosition = projectileWeapon.transform.position + new Vector3(0, heightProjectile, 0); // Adjust Y-offset (1.5f) as needed

        // Instantiate the projectile
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnLocation.transform.position, Quaternion.identity);

        // Calculate direction toward the player
        Vector3 direction = (playerTransform.position - projectileSpawnLocation.transform.position).normalized;

        // Set the projectile's velocity
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * projectileSpeed;
        }

        Debug.Log("Projectile Fired");
    }

    public void PlayerKilled()
    {
        playerTransform = null;
    }
}
