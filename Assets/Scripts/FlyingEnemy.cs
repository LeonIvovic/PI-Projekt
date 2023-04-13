using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
	public float speed;
    public float lineOfSite;
    public float bulletRange;
    public GameObject bullet;
    public GameObject bulletParent;
    public float fireRate = 1f;
    public float nextFireTime;
    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
       player = GameObject.FindGameObjectWithTag("Player").transform; 
    }

    // Update is called once per frame
    void Update()
    {
        // dodajemo range za pracenje

          float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        if(distanceFromPlayer < lineOfSite && distanceFromPlayer>bulletRange)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.position, speed * Time.deltaTime); 
        }

        else if(distanceFromPlayer<bulletRange && nextFireTime < Time.time){
            Instantiate(bullet, bulletParent.transform.position, Quaternion.identity);
            nextFireTime = Time.time + fireRate;
        }
        //transform.position = Vector2.MoveTowards(this.transform.position, player.position, speed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSite);

    }
}
