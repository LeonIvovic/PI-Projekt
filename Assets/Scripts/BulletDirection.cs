using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDirection : MonoBehaviour
{
    GameObject target;
    public float bulletSpeed;
    Rigidbody2D bulletRB;
    private GameObject go;
    public PlayerController scriptb;

    void Start()
    {
        go = GameObject.Find("Player");
        bulletRB = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        Vector2 moveDir = (target.transform.position - transform.position).normalized * bulletSpeed;
        bulletRB.velocity = new Vector2(moveDir.x, moveDir.y);
        Destroy(this.gameObject, 2);
    }
    private void OnCollisionEnter2D(Collision2D collision)

    {
        if(collision.collider.name != "Player"){
            print(collision.collider.name);
            return;
        }
        
        go.GetComponent<PlayerController>().TakeDamage(2);
        Destroy(this.gameObject);
    }
    
}
