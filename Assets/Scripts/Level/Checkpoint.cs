using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // Checkpoint can only be activated once
    bool actiavted = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !actiavted)
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            GameManager.GetInstance().SetCheckPoint(transform.position, player.GetHealth(), player.GetMaxHealth());
            actiavted = true;

            SpriteRenderer sprite;
            if (TryGetComponent(out sprite))
            {
                sprite.color = Color.green;
            }
        }
    }
}
