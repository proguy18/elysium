using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterStats))]
public class PlayerCombat : CharacterCombat
{
    public KeyCode attack = KeyCode.Mouse0; // Placeholder for attack change to mouse or as needed

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(attack))
            Attack();
    }

    // protected override void PlayAttackAnimation()
    // {
    //     m_Animator.SetTrigger("Attack_2");
    // }
}
