using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{

    [Header ("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    //dsdssd
    [Header("Enemy")]
    [SerializeField] private Transform enemy;


    [Header("Movement parameters")]
    [SerializeField] private float speed;
    private Vector3 initScale;
    private bool movingLeft;

    [Header("Idle behaviour")]
    [SerializeField ]private float idleDuration;
    private float idleTimer;

    [Header("Enemy Animator")]
    [SerializeField] private Animator anim;

    private void Awake()
    {
        initScale = enemy.localScale;
        
    }


    private void OnDisable()
    {
        anim.SetBool("movement", false);
    }


    private void Update()
    {
        if (movingLeft)
        {
            if (enemy.position.x >= leftEdge.position.x)
            MoveInDirection(-1);
            else
            {
                DirectionChange();
            }
        }

        else
        {
            if (enemy.position.x <= rightEdge.position.x)
            MoveInDirection(1);
            else
            {
                DirectionChange();
            }
        }

    }
    private void DirectionChange()
    {
        anim.SetBool("movement", false);

        idleTimer += Time.deltaTime; 

        if (idleTimer > idleDuration)
        {
            movingLeft = !movingLeft;
        }
      
    }


    private void MoveInDirection(int _direction)
    {
        idleTimer = 0;
        anim.SetBool("movement", true);
        //Debug.Log("prije " + enemy.localScale.x);
        //make enemy face right direction
        enemy.localScale = new Vector3(Mathf.Abs(enemy.localScale.x) * _direction, initScale.y, initScale.z);

        //Debug.Log("poslije " + enemy.localScale.x);
        //move enemy
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction*speed
            , enemy.position.y, enemy.position.z); 


    }


}
