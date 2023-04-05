using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameManager gameManager;
    private Transform t;
    [SerializeField] private int health;

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
}
