using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{

    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;
    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;
    [Header("Parent")]
    [SerializeField] private Transform odparents;
   
    private float cooldownTimer = Mathf.Infinity;
    //reference
    private Animator anim;
    private PlayerController player;
    private EnemyPatrol enemyPatrol;
        
    private void Awake()
    {

        odparents = transform.parent.GetComponent<Transform>();
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
        
    }    
        
        
        
    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("meleeatck");
                
            }

        }

        if (enemyPatrol != null)
        {
            enemyPatrol.enabled = !PlayerInSight();
        }


    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * odparents.localScale.x *colliderDistance,new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z
           ) ,0,Vector2.left, 0);

        if (hit.transform != null && hit.transform.CompareTag("Player"))
        {
            player = hit.transform.GetComponent<PlayerController>();
            return true;
        }

        return false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube (boxCollider.bounds.center + transform.right*range * odparents.localScale.x * colliderDistance, new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z
           ));
    }

    private void DamagePlayer()
    {
        if (PlayerInSight())
        {
            player.TakeDamage(damage);
        }

    }


}
