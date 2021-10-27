using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/* Sits on all InventorySlots. */

public class InventorySlot : MonoBehaviour {

	public Item item;	// Current item in the slot

	// Add item to the slot
	public void AddItem (Item newItem)
	{
		Image icon = transform.GetChild(0).GetComponent<Image>();
		item = newItem;
		icon.enabled = true;
		icon.sprite = item.icon;

	}

	// Clear the slot
	public void ClearSlot ()
	{
		Image icon = transform.GetChild(0).GetComponent<Image>();
		
		item = null;

		icon.sprite = null;
		icon.enabled = false;
	}

	// If the remove button is pressed, this function will be called.
	public void RemoveItemFromInventory ()
	{
		Inventory.instance.Remove(item);
	}

	/*public void moveItem () {
        GameObject canvas = GameObject.Find("Canvas");
		Transform inv = canvas.transform.Find("Inventory");
        Transform character = inv.transform.Find("Character");
		Transform weapons = character.transform.Find("Weapons");
		InventorySlot SwordSlot = weapons.transform.Find("SwordSlot").GetComponent<InventorySlot>();
		InventorySlot RingSlot = weapons.transform.Find("RingSlot").GetComponent<InventorySlot>();
		InventorySlot PotionSlot = weapons.transform.Find("PotionSlot").GetComponent<InventorySlot>();
		PlayerInventory inventory = inv.GetComponent<PlayerInventory>();

		if((item.type == Item.Type.Sword) && (!inventory.swordEquipped)){
			SwordSlot.AddItem(item);
			ClearSlot();
		}
		if((item.type == Item.Type.Ring) && (!inventory.ringEquipped)){
			RingSlot.AddItem(item);
			ClearSlot();
		}
		if((item.type == Item.Type.Potion) && (!inventory.potionEquipped)){
			PotionSlot.AddItem(item);
			ClearSlot();
		}
    }*/
}
