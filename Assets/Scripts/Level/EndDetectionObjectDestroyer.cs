using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDetectionObjectDestroyer : MonoBehaviour
{
    // Start is called before the first frame update
   private void OnTriggerEnter2D(Collider2D collision)
    {
        //kada dode do sudara PlatformDetectera i GroundSpawnera uništi PlatformDetecter
        if (collision.gameObject.CompareTag("GroundSpawner"))
        {
            Destroy(gameObject);
        }
        
    }
}
