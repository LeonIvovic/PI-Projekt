using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameManager gameManager;
    private Transform t;
    [SerializeField] private int health;
    [SerializeField] private int maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        t = GetComponent<Transform>();
        gameManager = GameManager.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        if (t.position.y < -10)
        {
            gameManager.PlayerDeath();
        }
    }

    public void TakeDamage(int dmg)
    {
        // Ignore negative damage, we don't want accidental healing
        // Healing can be done in a different function
        if (dmg > 0)
        {
            health -= dmg;

            if (health <= 0)
            {
                gameManager.PlayerDeath();
            }
        }
    }

    public void HealDamage(int heal)
    {
        // Ignore negative heal, we don't want accidental damage
        if (heal > 0)
        {
            health = Mathf.Min(health + heal, maxHealth);

            if (health <= 0)
            {
                gameManager.PlayerDeath();
            }
        }
    }

    public void AddMaxHealth(int hp)
    {
        // Ignore negative hp, we don't want accidental damage
        if (hp > 0)
        {
            health += hp;
            maxHealth = Mathf.Max(health, maxHealth);

            if (health <= 0)
            {
                gameManager.PlayerDeath();
            }
        }
    }

    public void LoadCheckpoint(int currentHp, int maxHp)
    {
        health = currentHp;
        maxHealth = maxHp;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public int GetHealth()
    {
        return health;
    }
}
