using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    public GameObject Ground1,Ground2,Ground3;
    public GameObject Player;
    private Rigidbody2D rigidBody;
    bool hasGround=true;
    
    [SerializeField]
     float speed = 10f; // The speed of the movement
    [SerializeField]
     float duration = 30f; 
    // Start is called before the first frame update
    void Start()
    {
       

         StartCoroutine(MoveRightForDuration());
    }

    // Update is called once per frame
    void Update()
    {
     

    }

      private IEnumerator MoveRightForDuration() {
        float timeElapsed = 0f;

        while (timeElapsed < duration) {
            transform.Translate(speed * Time.deltaTime, 0, 0, Space.Self);
            timeElapsed += Time.deltaTime;
            if(!hasGround){
            SpawnGround();
            hasGround=true;
        }
            yield return null;
        }
    }
  

    public void SpawnGround(){
        int randomNum = Random.Range(1,4);
        int x = Random.Range(3,7);
        float y = Random.Range(-4,2);

        if(randomNum==1){
            Instantiate(Ground1,new Vector3(transform.position.x+x,y,0),Quaternion.identity);
            
        }
        if(randomNum==2){
            Instantiate(Ground2,new Vector3(transform.position.x+x,y,0),Quaternion.identity);
        }
        if(randomNum==3){
            Instantiate(Ground3,new Vector3(transform.position.x+x,y,0),Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.CompareTag("Ground")){
            hasGround=true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision){
        if(collision.gameObject.CompareTag("Ground")){
            hasGround=false;
        }
    }
}
