using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float maxHealth = 1000f;
    private float health;
    private Slider slider;

    [Header("Animations")]
    private Animator animator;

    private bool isDead = false;

    private int attackID = 0;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        health = maxHealth;
        slider = GetComponentInChildren<Slider>();
        slider.maxValue = maxHealth;
        slider.value = health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetAttackID()
    {
        return attackID;
    }

    public void SetAttackID(int newAttackId)
    {
        attackID = newAttackId;
    }

    public void ApplyDamage(float damage, Vector3 hitDirection)
    {
        health -= damage;

        if (GetComponent<PushEffect>() != null)
        {
            GetComponent<PushEffect>().ApplyPushback(hitDirection);
        }

        slider.value = health;
        if (health <= 0 && !isDead)
        {
            StartCoroutine(Die()); 
        }
    }

    private IEnumerator Die()
    {
        isDead = true;
        animator.SetTrigger("Death");  
        yield return new WaitForSeconds(2f); 
        Destroy(gameObject);
        // Speel smoke particle effect
    }
}
