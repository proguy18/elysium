using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    public float lookRadius = 10f;

    public int maxHealth = 100;
    int currentHealth;
    
    Transform target;
    NavMeshAgent agent;

    void Start()
    {
        target = PlayerManager.instance.player.transform;   
        agent = GetComponent<NavMeshAgent>();
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
        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= lookRadius) 
        {
            agent.SetDestination(target.position);

            if (distance <= agent.stoppingDistance) 
            {
                // Attack the target
                // Face the target
                FaceTarget();

            }
        }
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
}
