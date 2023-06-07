using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpike : MonoBehaviour
{

    private Rigidbody2D myBody;

    [SerializeField]
    private LayerMask playerLayer;

    private RaycastHit2D playerHit;
    private bool playerDetected;

    private void Awake(){

        myBody = GetComponent<Rigidbody2D>();

    }

    private void Update(){

        DetectPlayer();

    }

    void DetectPlayer(){
    if (playerDetected) {
        return;
    }
    
    //pomicanje zrake detekcije (trenutno -10f u odnosu na spike)
    Vector2 raycastOrigin = new Vector2(transform.position.x - 6f, transform.position.y);

    playerHit = Physics2D.Raycast(raycastOrigin, Vector2.down, 10f, playerLayer);

    Debug.DrawRay(raycastOrigin, Vector2.down * 10f, Color.red);

    if (playerHit && playerHit.collider.CompareTag("Player")) {
        playerDetected = true;
        myBody.gravityScale = 1f;
        
    }
}


    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.CompareTag("Player")) {
        Destroy(gameObject);
        }
    }

    


}
