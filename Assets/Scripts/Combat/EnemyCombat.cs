using System;
using UnityEngine;

namespace Combat
{
    public class EnemyCombat : CharacterCombat
    {
        private Transform target;
        private EnemyAudioController audioController;
        
        void Start()
        {
            // target = PlayerManager.instance.player.transform;
            target = GameObject.Find("Player(Clone)").transform;
            audioController = gameObject.GetComponent<EnemyAudioController>();
        }
        
        protected override void Update()
        {
            base.Update();
            float distance = Vector3.Distance(target.position, transform.position);

            // Attacks target if target is within attack range
            if (distance <= attackRange)
            {
                // Play attack sound
                if (attackCooldown <= 0f && gameObject.GetComponent<EnemyController>().IsAlive())
                {
                    audioController.attackSound();
                    Attack();
                }
            }
        }
    }
}