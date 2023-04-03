using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameManager gameManager;
    private Transform t;

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
}
