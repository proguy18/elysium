using UnityEngine;

namespace Controllers
{
    public class SpiderController : EnemyController
    {
        protected override void PlayAttackAnimation()
        {
            m_Animator.SetInteger("AttackIndex", Random.Range(0,3));
            m_Animator.SetTrigger("Attack");
        }
    }
}