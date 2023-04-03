using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelLoader : MonoBehaviour
{
    private GameManager gameManager;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameManager = GameManager.GetInstance();
            gameManager.LoadNextLevel();
        }
    }

/*    private void OnColli(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameManager = GameManager.GetInstance();
            gameManager.LoadNextLevel();
        }
    }*/
}
