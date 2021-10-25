namespace Controllers
{
    public class SkeletonController : EnemyController
    {
        protected override void PlayAttackAnimation()
        {
            m_Animator.SetTrigger("Attack");
        }
    }
}