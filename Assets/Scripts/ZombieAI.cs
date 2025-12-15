using UnityEngine;

public class ZombieAI : MonoBehaviour
{
    public float speed = 2f;
    public float chaseRange = 5f;
    public float attackRangeX = 0.8f;
    public float attackRangeY = 0.8f;
    public float attackCooldown = 1.5f;
    public float attackDamage = 5f;
    public float health = 100f;

    private Transform player;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private float lastAttackTime;
    private float currentHealth;

    void Start()
    {
        currentHealth = health;
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(name + " took " + damage + " damage. HP: " + currentHealth);

        StartCoroutine(FlashRed());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(name + "died!");

        GetComponent<Collider2D>().enabled = false;

        if (GetComponent<ZombieAI>() != null)
        {
            GetComponent<ZombieAI>().enabled = false;
        }

        GetComponent<Animator>().enabled = false;

        transform.Rotate(0, 0, 90);

        this.enabled = false;
    }
    System.Collections.IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        Vector2 direction = (player.position - transform.position).normalized;

        float currentRequiredRange;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            currentRequiredRange = attackRangeX;
        }
        else
        {
            currentRequiredRange = attackRangeY;
        }

        if (distanceToPlayer < currentRequiredRange)
        {
            AttackPlayer();
        }
        else if (distanceToPlayer < chaseRange)
        {
            ChasePlayer();
        }
        else
        {
            StopChasing();
        }
    }

    void ChasePlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        Vector2 direction = (player.position - transform.position).normalized;

        anim.SetFloat("Horizontal", direction.x);
        anim.SetFloat("Vertical", direction.y);
        anim.SetFloat("Speed", 1);
    }

    void StopChasing()
    {
        anim.SetFloat("Speed", 0);
    }

    void AttackPlayer()
    {
        anim.SetFloat("Speed", 0);

        if (Time.time > lastAttackTime + attackCooldown)
        {
            anim.SetTrigger("Attack");
            lastAttackTime = Time.time;

            Debug.Log("Zombie Attacks!");
            if (player != null)
            {
                PlayerController playerHealth = player.GetComponent<PlayerController>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage((int)attackDamage);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRangeX);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRangeY);
    }
}
