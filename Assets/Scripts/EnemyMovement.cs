using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public enum EnemyType { Melee, Ranged }
    public EnemyType enemyType;

    [Header("Settings")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float attackRange = 2f;
    //[SerializeField] private float rangedAttackRange = 10f; // Ranged attack distance

    [Header("Attack")]
    [SerializeField] private float attackCooldown = 1.5f; // Time between attacks
    private float attackTimer;
    [SerializeField] AttackCollider attackCollider;

    [Header("References")]
    private Transform playerTransform;
    private Animator animator;



    void Start()
    {
        animator = GetComponent<Animator>();
        attackTimer = attackCooldown;

        if (playerTransform == null)
        {
            playerTransform = GameObject.FindAnyObjectByType<CharacterController>().transform;
        }

        attackCollider.SetPower(50);
    }

    void Update()
    {
        if (playerTransform == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        attackTimer -= Time.deltaTime;

        if (distanceToPlayer <= attackRange && attackTimer <= 0)
        {
            Attack();
            attackTimer = attackCooldown;
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
        transform.LookAt(new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z));
        animator.SetBool("Running", true);
    }

    private void Attack()
    {
        animator.SetTrigger("Attacking");
        animator.SetBool("Running", false);
        if (enemyType == EnemyType.Melee)
        {
            Debug.Log("Melee Attack");
        }
        else if (enemyType == EnemyType.Ranged)
        {
            Debug.Log("Ranged Attack");
            ShootProjectile();
        }
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
        // Placeholder for projectile shooting logic
        Debug.Log("Projectile Fired");
        // Instantiate a projectile and set its direction towards the player
    }

    public void PlayerKilled()
    {
        playerTransform = null;
    }
}
