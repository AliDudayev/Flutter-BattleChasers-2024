using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    private bool isAttacking = false;
    private int power = 50;
    private int attackID = 0;

    private Collider attackCollider;

    // Start is called before the first frame update
    void Start()
    {
        attackCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPower(int power)
    {
        this.power = power;
    }

    public void SetIsAttacking(bool attacking)
    {
        isAttacking = attacking;
        if(attacking)
        {
            attackID++;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (isAttacking)
        {
            Health health = other.GetComponent<Health>();
            if (health != null && attackID != health.GetAttackID())
            {
                Debug.Log("Hit: " + attackID);
                health.SetAttackID(attackID);
                health.ApplyDamage(power, transform.position);
            }
        }
    }

    public void SetSize(Vector3 newSize)
    {
        if (attackCollider is BoxCollider boxCollider)
        {
            boxCollider.size = newSize;
        }
    }
}
