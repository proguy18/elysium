using System;
using System.Collections;
using System.Collections.Generic;
using Combat;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(EnemyCombat))]
[RequireComponent(typeof(CharacterStats))]
public abstract class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    public float lookRadius = 10f;

    private EnemyAudioController enemyAudio;
    
    Transform target;
    NavMeshAgent agent;
    private CharacterCombat characterCombat;
    protected Animator m_Animator;

    private CharacterStats stats;

    private void Awake()
    {
        stats = GetComponent<CharacterStats>();
        characterCombat = GetComponent<CharacterCombat>();
    
        // target = PlayerManager.instance.player.transform;
        target = GameObject.Find("Player(Clone)").transform;
        // target = GameObject.Find("Player").transform;
        
        agent = GetComponent<NavMeshAgent>();
        
        enemyAudio = gameObject.GetComponent<EnemyAudioController>();

        m_Animator = GetComponent<Animator>();
    }
    void Start()
    {
        target = PlayerManager.instance.player.transform;
    }

    private void OnEnable()
    {
        stats.OnHealthReachedZero += Die;
        stats.OnDamaged += PlayOnHitAnimation;
        characterCombat.OnAttacking += PlayAttackAnimation;
    }

    private void OnDisable()
    {
        stats.OnHealthReachedZero -= Die;
        stats.OnDamaged -= PlayOnHitAnimation;
        characterCombat.OnAttacking -= PlayAttackAnimation;
    }
    
    private void PlayOnHitAnimation()
    {
        m_Animator.SetTrigger("Hit");
        enemyAudio.getHitSound();
    }

    void Die()
    {
        // Die animation
        m_Animator.SetInteger("DeathIndex", Random.Range(0,2));
        m_Animator.SetTrigger("Die");
        m_Animator.SetBool("hasDied", true);

        // Disable the enemy
        characterCombat.Toggle(false);
        Destroy(gameObject, 2.1f);
        
    }
    void Update () 
    {
        if(!m_Animator)
        {
            m_Animator  = gameObject.GetComponent<Animator>();
        }
        
        float distance = Vector3.Distance(target.position, transform.position);
        
        // Move to target if target is within look radius and is alive
        if (distance <= lookRadius && IsAlive()) 
        {
            agent.SetDestination(target.position);
            m_Animator.SetBool("Run", true);
            enemyAudio.movementSounds();

            if (distance <= agent.stoppingDistance) 
            {
                // Attack the target
                // Face the target
                FaceTarget();

            }
        }
        // Stop running when enemy is not travelling
        if (IsStationary())
        {
            m_Animator.SetBool("Run", false);
            enemyAudio.stopMainSound();
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

    bool IsStationary()
    {
        return agent.velocity.Equals(new Vector3(0, 0, 0));
    }

    public bool IsAlive()
    {
        return !m_Animator.GetBool("hasDied");
    }
    protected abstract void PlayAttackAnimation();
}
