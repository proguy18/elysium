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
        
        agent = GetComponent<NavMeshAgent>();
        
    }

    public void TakeDamage(int damage) 
    {
        currentHealth -= damage;

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


    }
}
