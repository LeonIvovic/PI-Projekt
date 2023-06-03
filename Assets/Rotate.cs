using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    public BoxCollider2D collider1;
    public BoxCollider2D collider2;
    [SerializeField] private float timeout;
    private float cooldownTimer = Mathf.Infinity;
    private void Start()
    {
        collider1 = GetComponent<BoxCollider2D>();
        GameObject playerObject = GameObject.FindWithTag("Player");
        PlayerController playerController = playerObject.GetComponent<PlayerController>();

        // Get the collider component from the player object
        collider2 = playerController.GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        transform.Rotate(0, 0, 360 * speed * Time.deltaTime);
        if (collider1.bounds.Intersects(collider2.bounds))
        {
            Debug.Log("Collided!");
            GameObject playerObject = GameObject.FindWithTag("Player");
            PlayerController playerController = playerObject.GetComponent<PlayerController>();

            if (cooldownTimer >= timeout)
            {
                
                cooldownTimer = 0;
                playerController.TakeDamage(damage);

            }
            
            // Perform desired actions when the colliders have collided
        }
    }
    [SerializeField] private int damage;


    protected void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("radi");
        if (collision.CompareTag("Player"))
            collision.GetComponent<PlayerController>().TakeDamage(damage);

      

    }

}