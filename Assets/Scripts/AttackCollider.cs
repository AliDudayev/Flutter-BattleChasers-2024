using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    private bool isAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetIsAttacking(bool attacking)
    {
        isAttacking = attacking;
    }
    private void OnTriggerStay(Collider other)
    {
        if (isAttacking)
        {
            Health health = other.GetComponent<Health>();
            Debug.Log("Health: " + health);
            if (health != null)
            {
                health.ApplyDamage(100);
            }
        }
    }
}
