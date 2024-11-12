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

    [Header("Health decrease speed")]
    [SerializeField] float lerpSpeed = 5f;

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
        slider.value = Mathf.MoveTowards(slider.value, health, lerpSpeed * Time.deltaTime);
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
        // Dit zorgt ervoor dat health niet onder 0 kan gaan en niet boven maxHealth kan gaan
        health = Mathf.Clamp(health - damage, 0, maxHealth);

        if (GetComponent<PushEffect>() != null)
        {
            GetComponent<PushEffect>().ApplyPushback(hitDirection);
        }

        if (health <= 0 && !isDead)
        {
            StartCoroutine(Die()); 
        }
    }

    private IEnumerator Die()
    {
        animator.SetBool("IsDead", true);

        if (gameObject.GetComponent<EnemyMovement>() != null)
        {
            gameObject.GetComponent<EnemyMovement>().enabled = false;
        }
        else if (gameObject.GetComponent<CharacterController>() != null)
        {
            gameObject.GetComponent<CharacterController>().enabled = false;
        }
        gameObject.GetComponent<BoxCollider>().enabled = false;

        if (gameObject.GetComponent<Rigidbody>() != null)
        {
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }

        isDead = true;
        yield return new WaitForSeconds(5f); 
        Destroy(gameObject);
        // Speel smoke particle effect
    }
}
