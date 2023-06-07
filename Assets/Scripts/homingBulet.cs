using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class homingBulet : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    Transform player;
    private GameObject go;
    void Start()
    {
        go = GameObject.Find("Player");
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Destroy(this.gameObject, 2);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)

    {
        if(collision.collider.name != "Player"){
            Destroy(this.gameObject);
            return;
        }
        
        go.GetComponent<PlayerController>().TakeDamage(2);
        Destroy(this.gameObject);
    }
}
