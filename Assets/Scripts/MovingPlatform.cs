using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform posA;
    public Transform posB;
    public int speed;
    private Vector2 targetPos;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        targetPos = posB.position;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Vector2.Distance(transform.position, posA.position)< .1f){
            targetPos = posB.position;
        } 

        if(Vector2.Distance(transform.position, posB.position)< .1f){
            targetPos = posA.position;
        }

        rb.velocity = (targetPos - rb.position).normalized * speed * Time.fixedDeltaTime;
    }
}
