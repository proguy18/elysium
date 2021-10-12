using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;
    public override void Interact()
    {
        base.Interact();

        PickUp();
    }

    void PickUp () 
    {
        Debug.Log("Picking up " + item.name);
        // Add item to inventory
        bool wasPickedUp = Inventory.instance.Add(item);


        // Destroy game object
        if (wasPickedUp)
            Destroy(gameObject);
        // Set ui TEXT to false
        uiObject.SetActive(false);
    }
}
