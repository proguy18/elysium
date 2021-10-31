using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterStats))]
public class PlayerCombat : CharacterCombat
{
    [SerializeField] private KeyCode attack = KeyCode.Mouse1; // Placeholder for attack change to mouse or as needed

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(attack) && 
            !PauseMenu.IsPaused && 
            !PlayerInventory.InventoryIsActive)
            Attack();
    }

}
