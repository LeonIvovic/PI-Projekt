using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform Player;
    private Vector3 tempPos;
    void Start()
    {
        Player = GameObject.FindWithTag("Player").transform;
        tempPos = transform.position - Player.transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        tempPos = transform.position;
        tempPos.x = Player.position.x;
        
        transform.position = tempPos;
        */

        transform.position = new Vector3(Player.transform.position.x + tempPos.x,transform.position.y,transform.position.z);
    }
}
