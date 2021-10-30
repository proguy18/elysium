using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public abstract class CharacterCombat : MonoBehaviour
{
    public event Action OnAttacking;
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] protected float attackCooldown;
    [SerializeField] private Transform attackPoint;
    [SerializeField] protected float attackRange = 2f;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] protected Animator m_Animator;
    private CharacterStats stats;
    private bool isEnabled = true;
    
    private void Awake()
    {
        stats = GetComponent<CharacterStats>();
        m_Animator  = gameObject.GetComponent<Animator>();
    }
    
    protected virtual void Update()
    {
        attackCooldown -= Time.deltaTime;
    }

    public void Attack()
    {
        if(!isEnabled)
            return;
        if (attackCooldown <= 0f)
        {
            // Play attack animation
            // PlayAttackAnimation();
            if (OnAttacking != null) 
                OnAttacking();

            // Detect enemies in range of attack
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

            // Damage enemies
            foreach(Collider enemy in hitEnemies)
                enemy.GetComponent<CharacterStats>().TakeDamage(stats.damage.GetValue());
            attackCooldown = 1 / attackSpeed;
        }
    }
    
    // protected abstract void PlayAttackAnimation();
    
    void OnDrawGizmosSelected() 
    {
        if (attackPoint == null) 
            return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void Toggle(bool enable)
    {
        isEnabled = enable;
    }

    public float getCooldown()
    {
        return attackCooldown;
    }
}
