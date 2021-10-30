using System;
using UnityEngine;

namespace Combat
{
    public class EnemyCombat : CharacterCombat
    {
        private Transform target;
        
        void Start()
        {
            target = PlayerManager.instance.player.transform;
        }
        
        protected override void Update()
        {
            base.Update();
            float distance = Vector3.Distance(target.position, transform.position);

            // Attacks target if target is within attack range
            if (distance <= attackRange)
            {   
                // Play attack sound
                if (attackCooldown <= 0f)
                {
                    gameObject.GetComponent<EnemyAudioController>().attackSound();
                }
                Attack();
            }
        }
    }
}