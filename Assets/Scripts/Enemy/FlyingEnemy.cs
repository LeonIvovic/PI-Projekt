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
    private Vector2 player1;
    //private double angleConst = 180.0;
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
                
                /*Vector2 vector = (bulletParent.transform.position -player.position).normalized;
                //float yCoord = bulletParent.transform.y -player.y;
                Vector3 xyz = new Vector3(vector.x * 180.0f, vector.y * 90.0f, 1);
                print("  " + vector.x + "  " + vector.y + "\n");
                Quaternion newIdentity = Quaternion.Euler(xyz);
                //Quaternion newIdentity = Quaternion.AngelAxis(vector.x * 90.0f, 0);*/
            //GameObject.Instantiate(instance, this.transform.position, newRotation);

            Instantiate(bullet, bulletParent.transform.position, Quaternion.identity);
            nextFireTime = Time.time + fireRate;
            //transform.position = Vector2.MoveTowards(this.transform.position, player.position, speed * Time.deltaTime);
        }
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSite);

    }
}
