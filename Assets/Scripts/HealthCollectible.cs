using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
   [SerializeField] private int healthValue;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerController>().AddMaxHealth(healthValue);
            gameObject.SetActive(false);    
        }

    }

}
