using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

/* This object manages the inventory UI. */

public class PlayerInventory : MonoBehaviour {

	public GameObject inventoryUI;	// The entire UI
	public Transform storage;	// The parent object of all the items
    public KeyCode inventoryKey = KeyCode.I;

    public Item swordEquipped = null;
    public Item ringEquipped = null;
    public Item potionEquipped = null;
    public int space = 24;	
    private int slotsOccupied = 0;

	void Start ()
	{
		inventoryUI.SetActive(false);
	}

	// Check to see if we should open/close the inventory
	void Update ()
	{
		if (Input.GetKeyDown(inventoryKey))
		{
			if (Time.timeScale == 0) 
			{ 
				Time.timeScale = 1;
				Cursor.visible = false;
			}
			else 
			{ 
				Time.timeScale = 0;
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
			}
			inventoryUI.SetActive(!inventoryUI.activeSelf);
		}

	}

    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.tag == "Item"){
            Item item = collision.collider.gameObject.GetComponent<Item>();       
            if (slotsOccupied < space) {
                AddToInv(item);
                Destroy(collision.collider.gameObject);
                slotsOccupied++;
            }
        }
    }


    public void AddToInv(Item item)
	{
        GameObject canvas = GameObject.Find("Canvas");
        Transform inv = canvas.transform.Find("Inventory");
        Transform storage = inv.transform.Find("Storage");
		Item[] slots = storage.GetComponentsInChildren<Item>();
		for (int i = 0; i < slots.Length; i++)
		{
			if (slots[i].icon == null)
			{
                FillSlot(slots[i], item);
                return;
            }
		}
	}

    public void moveItem (GameObject button) {
        GameObject canvas = GameObject.Find("Canvas");
        Transform inv = canvas.transform.Find("Inventory");
        Transform character = inv.transform.Find("Character");
        Transform weapons = character.transform.Find("Weapons");
        Item SwordSlot = weapons.transform.Find("SwordSlot").GetComponent<Item>();
        Item RingSlot = weapons.transform.Find("RingSlot").GetComponent<Item>();
        Item PotionSlot = weapons.transform.Find("PotionSlot").GetComponent<Item>();
        PlayerInventory inventory = inv.GetComponent<PlayerInventory>();
        Item item = button.transform.parent.gameObject.GetComponent<Item>();
        if(item.icon == null) { return; }
        if(button.transform.parent.parent.name == "Storage"){
            if((item.type == Item.Type.Sword) && (!swordEquipped)){
                FillSlot(SwordSlot, item);
                ClearSlot(item);
                swordEquipped = item;
            }
            if((item.type == Item.Type.Ring) && (!ringEquipped)){
                FillSlot(RingSlot, item);
                ClearSlot(item);
                ringEquipped = item;
            }
            if((item.type == Item.Type.Potion) && (!potionEquipped)){
                FillSlot(PotionSlot, item);
                ClearSlot(item);
                potionEquipped = item;
            }
        }
        else {
            AddToInv(item);
            if(item.type == Item.Type.Sword){
                ClearSlot(SwordSlot);
                swordEquipped = null;
            }
            if(item.type == Item.Type.Ring){
                ClearSlot(RingSlot);
                ringEquipped = null;
            }
            if(item.type == Item.Type.Potion){
                ClearSlot(PotionSlot);
                potionEquipped = null;
            }
        }
        
    }


	public void FillSlot (Item slot, Item newItem)
	{
		slot.name = newItem.name;
        slot.icon = newItem.icon;
        slot.type = newItem.type;
        Image icon = slot.gameObject.transform.GetChild(0).GetComponent<Image>();
        icon.enabled = true;
        icon.sprite = newItem.icon;

	}

    public void ClearSlot(Item item)
	{
		item.name = null;
        item.icon = null;
        item.type = Item.Type.Sword;
        Image icon = item.gameObject.transform.GetChild(0).GetComponent<Image>();
        icon.enabled = false;
        icon.sprite = null;
	}



}