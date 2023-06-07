using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLeftSideBoundaries : MonoBehaviour
{

   void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Boundary"))
    {
        float boundaryRight = collision.gameObject.GetComponent<Collider2D>().bounds.max.x;
        float offset = 0.1f; // adjust this value to prevent player from getting stuck

        transform.position = new Vector2(boundaryRight + offset, transform.position.y);
    }
}

}
