using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterStats))]

public class PlayerCombat : CharacterCombat
{
    public KeyCode attack = KeyCode.M; // Placeholder for attack change to mouse or as needed
    public float attackSpeed = 1f;
    private float attackCooldown = 0f;
    public LayerMask enemyLayers;
    public float attackRange = 0.5f;

    public Transform attackPoint;
    
    public AudioClip swordSwing;
    private AudioSource _audioSource;

    Animator m_Animator;

    CharacterStats myStats;

    // Start is called before the first frame update
    void Start()
    {
        myStats = GetComponent<CharacterStats>();
        m_Animator  = gameObject.GetComponent<Animator>();
        _audioSource = gameObject.GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(attack)) {
            Attack();
            _audioSource = gameObject.GetComponent<AudioSource>();
            Debug.Log(_audioSource.clip.ToString());
            if (_audioSource.clip != swordSwing)
            {
                _audioSource.Stop();
                _audioSource.clip = swordSwing;
                _audioSource.volume = 1f;
                _audioSource.Play();
            }
            Debug.Log(_audioSource.clip.ToString());
            _audioSource.Play();

        }

        attackCooldown -= Time.deltaTime;
    }

    public override void Attack () 
    {
        if (attackCooldown <= 0f)
        {
            Debug.Log("Attack done");
            // Play attack animation
            m_Animator.SetTrigger("Attack_2");

            // Detect enemies in range of attack
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

            // Damage enemies
            foreach(Collider enemy in hitEnemies) 
            {
                enemy.GetComponent<EnemyController>().TakeDamage(myStats.damage.GetValue());

            }
            attackCooldown = 1 / attackSpeed;
        }
    }

    void OnDrawGizmosSelected () 
    {
        if (attackPoint == null) 
            return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
