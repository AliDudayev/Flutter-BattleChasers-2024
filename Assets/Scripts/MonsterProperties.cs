using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterProperties : MonoBehaviour
{
    //public float strength = 100f;
    //public float maxHealth = 1000f;
    //private float health;
    //private float stamina = 90f;

    //private Slider slider;

    //private void Start()
    //{
    //    health = maxHealth;

    //    slider = GetComponentInChildren<Slider>();

    //    slider.maxValue = maxHealth;
    //    slider.value = health;
    //}

    //public void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.Space))
    //    {
    //        Attack(10);
    //    }
    //}

    //// You can add more properties or methods here, if needed
    //public void ApplyDamage(float damage)
    //{
    //    health -= damage;
    //    slider.value = health;

    //    if (health <= 0)
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    //public void Attack(float staminaCost)
    //{
    //    stamina -= staminaCost;
    //    ApplyDamage(strength * (staminaCost / 10) * 1.1f);
    //    Debug.Log(strength * (staminaCost / 10) * 1.1f);
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
