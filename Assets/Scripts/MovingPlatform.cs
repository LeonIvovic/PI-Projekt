using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
  public Transform posA;
public Transform posB;
public int Speed;
Vector2 targetPos;

private Transform playerTransform;
private bool isOnPlatform = false;
// Start is called before the first frame update
void Start()
{
    targetPos = posB.position;
}

// Update is called once per frame
void Update()
{
    if(Vector2.Distance(transform.position, posA.position) < .1f){
        targetPos = posB.position;
    } 
    if(Vector2.Distance(transform.position, posB.position) < .1f){
        targetPos = posA.position;
    }
    transform.position = Vector2.MoveTowards(transform.position, targetPos, Speed * Time.deltaTime);
}
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")){
            collision.transform.SetParent(this.transform);
        }
    }

     private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")){
            collision.transform.SetParent(null);
        }
    }*/
    void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.gameObject.CompareTag("Player"))
    {
        playerTransform = collision.gameObject.transform;
        isOnPlatform = true;
    }
}

void OnTriggerExit2D(Collider2D collision)
{
    if (collision.gameObject.CompareTag("Player"))
    {
        playerTransform = null;
        isOnPlatform = false;
    }
}
}
