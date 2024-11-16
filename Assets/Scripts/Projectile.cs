using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifetime = 10f;

    void Start()
    {
        Destroy(gameObject, lifetime); // Destroy after lifetime
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Apply damage to the player
            Health playerHealth = other.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.ApplyDamage(damage, transform.position);
            }
            Destroy(gameObject); // Destroy projectile on hit
        }
    }
}
