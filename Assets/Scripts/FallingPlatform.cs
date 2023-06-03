using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float fallDelay = 0.5f;
    private float destroyDelay = 2f;

    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Player")){
            StartCoroutine(Fall());
        }
    }
    private IEnumerator Fall(){
        yield return new WaitForSeconds(fallDelay);
        Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation + ((int)RigidbodyConstraints2D.FreezePositionX);
        }
        Destroy(gameObject, destroyDelay);
    }
}
