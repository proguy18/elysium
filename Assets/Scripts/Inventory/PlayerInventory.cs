using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

/* This object manages the inventory UI. */

public class PlayerInventory : MonoBehaviour {

	private GameObject inventoryUI;	// The entire UI
	private Transform storage;	// The parent object of all the items
    public KeyCode inventoryKey = KeyCode.I;

    private bool swordEquipped = false;
    private bool ringEquipped = false;
    private bool potionEquipped = false;
    private int space = 24;	
    private int slotsOccupied = 0;
    private Transform inv;
    private Item SwordSlot;
    private Item RingSlot;
    private Item PotionSlot;
    private CharacterStats stats;

    private bool swordIsEquipped() {return swordEquipped;}
    private bool ringIsEquipped() {return ringEquipped;}
    private bool potionIsEquipped() {return potionEquipped;}



	void Start ()
	{
        GameObject canvas = GameObject.Find("Canvas");
        inv = canvas.transform.Find("Inventory");
        inventoryUI = inv.gameObject;
        Transform character = inv.transform.Find("Character");
        Transform weapons = character.transform.Find("Weapons");
        SwordSlot = weapons.transform.Find("SwordSlot").GetComponent<Item>();
        RingSlot = weapons.transform.Find("RingSlot").GetComponent<Item>();
        PotionSlot = weapons.transform.Find("PotionSlot").GetComponent<Item>();
        GameObject player = GameObject.Find("Player");
        stats = player.GetComponent<CharacterStats>();
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

    public void clickSlot (GameObject button) {    
        PlayerInventory inventory = inv.GetComponent<PlayerInventory>();
        Item item = button.transform.parent.gameObject.GetComponent<Item>();
        if(item.icon == null) { return; }
        if(button.transform.parent.parent.name == "Storage"){
            if((item.type == Item.Type.Sword) && (!swordIsEquipped())){
                FillSlot(SwordSlot, item);
                ClearSlot(item);
                swordEquipped = true;
            }
            if((item.type == Item.Type.Ring) && (!ringIsEquipped())){
                FillSlot(RingSlot, item);
                ClearSlot(item);
                ringEquipped = true;
            }
            if((item.type == Item.Type.Potion) && (!potionIsEquipped())){
                FillSlot(PotionSlot, item);
                ClearSlot(item);
                potionEquipped = true;
            }
        }
        else {
            Debug.Log(item.type);
            AddToInv(item);
            if(item.type == Item.Type.Sword){
                ClearSlot(SwordSlot);
                swordEquipped = false;
            }
            if(item.type == Item.Type.Ring){
                ClearSlot(RingSlot);
                ringEquipped = false;
            }
            if(item.type == Item.Type.Potion){
                ClearSlot(PotionSlot);
                potionEquipped = false;
            }
        }
        
    }


	public void FillSlot (Item slot, Item newItem)
	{
        slot.icon = newItem.icon;
        slot.type = newItem.type;
        Image icon = slot.gameObject.transform.GetChild(0).GetComponent<Image>();
        icon.enabled = true;
        icon.sprite = newItem.icon;

	}

    public void ClearSlot(Item slot)
	{
        slot.icon = null;
        slot.type = Item.Type.Sword;
        Image icon = slot.gameObject.transform.GetChild(0).GetComponent<Image>();
        icon.enabled = false;
        icon.sprite = null;
	}



}