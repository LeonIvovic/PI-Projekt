using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class homingBulet : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    Transform player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }
}
