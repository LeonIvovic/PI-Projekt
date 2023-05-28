using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    public float speed;
    Vector3 tragetPos;

    public GameObject ways;
    public Transform[] wayPoints;
    int pointIndex;
    int pointCount;
    int direction = 1;

    public float waitDuration;
    int speedMultiplier = 1;
    
    private void Awake(){

        wayPoints = new Transform[ways.transform.childCount];

        for(int i = 0; i < ways.gameObject.transform.childCount;i++){
            wayPoints[i] = ways.transform.GetChild(i).gameObject.transform;
        }
    }

    private void Start(){

        pointCount = wayPoints.Length;
        pointIndex = 1;
        tragetPos = wayPoints[pointIndex].transform.position;

    }

    private void Update(){
        var step = speedMultiplier * speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position,tragetPos,step);

        if(transform.position == tragetPos){
            NextPoint();
        }
    }

    void NextPoint(){
        if(pointIndex == pointCount - 1){
            direction = -1;
        }

        if(pointIndex == 0){
            direction = 1;
        }

        pointIndex += direction;
        tragetPos = wayPoints[pointIndex].transform.position;
        StartCoroutine(WaitNextPoint());
    }

    IEnumerator WaitNextPoint(){
        speedMultiplier = 0;
        yield return new WaitForSeconds(waitDuration);
        speedMultiplier = 1;
    }

   
}
