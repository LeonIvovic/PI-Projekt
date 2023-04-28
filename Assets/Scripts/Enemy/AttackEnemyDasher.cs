using UnityEngine;

public class AttackEnemyDasher: MonoBehaviour
{

    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;
    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private Transform player;
    [Header("Parent")]
    [SerializeField] private Transform odparents;

    private float cooldownTimer = Mathf.Infinity;
    //reference
    private Animator anim;
    private PlayerController playerController;
    private EnemyPatrol enemyPatrol;

    private void Awake()
    {

        odparents = transform.parent.GetComponent<Transform>();
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = player.GetComponent<PlayerController>();
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
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * odparents.localScale.x * colliderDistance, new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z
           ), 0, Vector2.left, 0);

        return hit.transform != null && hit.transform.CompareTag("Player");
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * odparents.localScale.x * colliderDistance, new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z
           ));
    }

    // Called from animation
    private void DamagePlayer()
    {
        if (PlayerInSight())
        {
            playerController.TakeDamage(damage);
            if (odparents.position.x > player.position.x)
            {
                odparents.position = new Vector3(player.position.x + (float)0.4, odparents.position.y, odparents.position.z);
            }
            else
            {
                odparents.position = new Vector3(player.position.x - (float)0.4, odparents.position.y, odparents.position.z);
            }
            }

    }

    // Called from animation
    private void Dash()
    {
        odparents.position = new Vector3(( odparents.position.x +(player.position.x - odparents.position.x)/12), odparents.position.y, odparents.position.z);
    }

}
