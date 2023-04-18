using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private bool dead;

    private void Awake()
    {
        currentHealth = startingHealth;
    }

    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage / 10, 0,  startingHealth );
        
        if( currentHealth > 0)
        {
            //player hurt (trigerrati animaciju)
        }
        else
        {
            if (!dead)
            {
                //player dead (triggerati animaciju)
                GetComponent<PlayerMovement>().enabled = false;
                dead = true;
            }
            }


    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);




    }





}
