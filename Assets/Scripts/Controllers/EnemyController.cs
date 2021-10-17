using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    public float lookRadius = 10f;

    public float attackRange = 2f;

    public float attackSpeed = 1f;
    private float attackCooldown = 0f;

    public int maxHealth = 100;
    int currentHealth;
    
    Transform target;
    NavMeshAgent agent;
    private Animator m_Animator; 

    void Start()
    {
        target = PlayerManager.instance.player.transform;   
        agent = GetComponent<NavMeshAgent>();
        m_Animator = gameObject.GetComponent<Animator>();
        // Debug.Log("Enemy is currently " + currentHealth + " health.");
        
    }

    public void TakeDamage(int damage) 
    {
        currentHealth -= damage;
        Debug.Log("Enemy took " + damage + " damage.");
        Debug.Log("Enemy is currently " + currentHealth + " health.");

        // Play hurt animation

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    void Die() 
    {
        Debug.Log("Enemy died");
        // Die animation

        // Disable the enemy
        Destroy(gameObject);


    }
    void Update () 
    {
        
        if(!m_Animator)
        {
            m_Animator  = gameObject.GetComponent<Animator>();
        }
        
        float distance = Vector3.Distance(target.position, transform.position);
        // Debug.Log(agent.velocity);
        // Attacks target if target is within attack range
        if (distance <= attackRange)
        {
            Attack();
        }

        // Move to target if target is within look radius
        if (distance <= lookRadius) 
        {
            agent.SetDestination(target.position);
            m_Animator.SetBool("Run", true);


            if (distance <= agent.stoppingDistance) 
            {
                // Attack the target
                // Face the target
                FaceTarget();

            }
        }
        // Stop running when enemy is not travelling
        if (agent.velocity.Equals(new Vector3(0, 0, 0)))
        {
            m_Animator.SetBool("Run", false);
        }
        attackCooldown -= Time.deltaTime;
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3 (direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
    void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    void Attack()
    {
        if (attackCooldown <= 0f)
        {
            // Play attack animation
            m_Animator.SetTrigger("Attack");

            // Check if collides player

            // Damage player

            // Reset attackCooldown
            attackCooldown = 1 / attackSpeed;
        }
    }
}
