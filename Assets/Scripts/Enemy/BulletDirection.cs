using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDirection : MonoBehaviour
{
    GameObject target;
    public float bulletSpeed;
    Rigidbody2D bulletRB;

    void Start()
    {
       bulletRB = GetComponent<Rigidbody2D>();
       target = GameObject.FindGameObjectWithTag("Player");
        Vector2 moveDir = (target.transform.position - transform.position).normalized * bulletSpeed;
        bulletRB.velocity = new Vector2(moveDir.x, moveDir.y);
        /*if(target.transform.position - transform.position < new Vector2(0,0)){
            transform.Translate(transform.right);
        }*/

        // = Vector2.MoveTowards(transform.position, target.position, bulletSpeed * Time.deltaTime);
        //transform.LookAt(target.transform.position - transform.position);
        Destroy(this.gameObject, 2);
    }
    /*private void OnCollisionDestroy(Collision2D collision)
    {
        Destroy(collision.gameObject);

    }
*/    
}
