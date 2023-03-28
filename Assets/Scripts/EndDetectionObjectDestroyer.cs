using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDetectionObjectDestroyer : MonoBehaviour
{
    // Start is called before the first frame update
   private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("GroundSpawner"))
        {
            Destroy(gameObject);
        }
        
    }
}
