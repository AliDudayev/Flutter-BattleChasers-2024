using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float maxHealth = 1000f;
    private float health;
    [SerializeField] float score = 1;

    [Header("Alleen voor player")]
    [SerializeField] GameObject canvas;
    [SerializeField] Slider slider;

    [Header("Animations")]
    private Animator animator;

    private bool isDead = false;

    private int attackID = 0;

    [Header("Health decrease speed")]
    [SerializeField] float lerpSpeed = 5f;

    public delegate void DeathDelegate();
    public event DeathDelegate OnDeath;

    [SerializeField] GameObject deathParticle;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        health = maxHealth;
        if (gameObject.GetComponent<EnemyMovement>() != null || gameObject.GetComponent<DragonMovement>() != null)
        {

            slider = GetComponentInChildren<Slider>();
        }
        else if(gameObject.GetComponent<CharacterController>() != null)
        {
            canvas.SetActive(true);
        }
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

        if(gameObject.GetComponent<EnemyMovement>() != null)
        {
            int sound = UnityEngine.Random.Range(1, 3);
            if(sound == 1)
            {
                GetComponent<SoundManager>().PlaySoundRandomPitch("TakeDamage");
            }
            else
            {
                GetComponent<SoundManager>().PlaySoundRandomPitch("TakeDamage2");
            }
        }
        else if (gameObject.GetComponent<CharacterController>() != null)
        {
            GetComponent<SoundManager>().PlaySoundRandomPitch("TakeDamage");
            if (Application.platform == RuntimePlatform.Android)
            {
                Handheld.Vibrate();
            }
        }
        else
        {
            GetComponent<SoundManager>().PlaySoundRandomPitch("Pain");
        }


        if (GetComponent<PushEffect>() != null)
        {
            GetComponent<PushEffect>().ApplyPushback(hitDirection);
        }

        if (health <= 0 && !isDead)
        {
            isDead = true;
            StartCoroutine(Die()); 
        }
    }

    private IEnumerator Die()
    {
        animator.SetBool("IsDead", true);

        //Console.WriteLine("1");
        if (gameObject.GetComponent<EnemyMovement>() != null || gameObject.GetComponent<DragonMovement>() != null)
        {
            //Console.WriteLine("2");
            GameObject gameManager = GameObject.Find("GameManager");
            if (gameObject.GetComponent<DragonMovement>() != null)
            {
                //Console.WriteLine("3");
                gameObject.GetComponent<DragonMovement>().enabled = false;
                gameManager.GetComponent<GameEnd>().AddKilledDragon(gameObject.name);
            }
            else
            {
                //Console.WriteLine("4");
                int sound = UnityEngine.Random.Range(1, 3);
                if (sound == 1)
                {
                    GetComponent<SoundManager>().PlaySoundRandomPitch("Death");
                }
                else
                {
                    GetComponent<SoundManager>().PlaySoundRandomPitch("Death2");
                }

                gameObject.GetComponent<EnemyMovement>().enabled = false;
            }
            gameManager.GetComponent<GameEnd>().IncreaseKillCount();
            //Console.WriteLine("5");

            GameObject scoreObject = GameObject.Find("Points");
            scoreObject.GetComponent<Text>().text = (int.Parse(scoreObject.GetComponent<Text>().text) + score).ToString();

            OnDeath?.Invoke();
        }
        else if (gameObject.GetComponent<CharacterController>() != null)
        {
            GetComponent<SoundManager>().PlaySound("Death");

            gameObject.GetComponent<CharacterController>().enabled = false;
            FindAnyObjectByType<GameManager>().TriggerEnemiesWinAnimation();
        }
        //Console.WriteLine("6");
        if(gameObject.GetComponent<CapsuleCollider>() != null)
        {
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }

        if (gameObject.GetComponent<Rigidbody>() != null)
        {
            //Console.WriteLine("7");
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }

        if (gameObject.GetComponent<CharacterController>() != null)
        {
            canvas.SetActive(false);
            GameObject knightTarget = GameObject.Find("KnightTarget");
            Destroy(knightTarget);
        }

        yield return new WaitForSeconds(5f);

        if(gameObject.GetComponent<CharacterController>() == null)
        {
            Instantiate(deathParticle, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }


    private void FindScoreObject(string name) 
    {
        GameObject scoreObject = GameObject.Find(name);
        scoreObject.GetComponent<Text>().text = (int.Parse(scoreObject.GetComponent<Text>().text) + score).ToString();
    }
}
