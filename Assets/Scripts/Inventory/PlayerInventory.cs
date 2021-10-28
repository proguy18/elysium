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
    private Text statsDisplay;

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
        statsDisplay = character.transform.Find("Text").GetComponent<Text>();        
        inventoryUI.SetActive(false);
        updateText();
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
                AddModifiers(item);
                FillSlot(SwordSlot, item);
                ClearSlot(item);
                swordEquipped = true;                
            }
            if((item.type == Item.Type.Ring) && (!ringIsEquipped())){
                AddModifiers(item);
                FillSlot(RingSlot, item);
                ClearSlot(item);
                ringEquipped = true;
            }
            if((item.type == Item.Type.Potion) && (!potionIsEquipped())){
                AddModifiers(item);
                FillSlot(PotionSlot, item);
                ClearSlot(item);
                potionEquipped = true;
            }
        }
        else {
            AddToInv(item);
            if(item.type == Item.Type.Sword){
                RemoveModifiers(item);
                ClearSlot(SwordSlot);
                swordEquipped = false;
            }
            if(item.type == Item.Type.Ring){
                RemoveModifiers(item);
                ClearSlot(RingSlot);
                ringEquipped = false;
            }
            if(item.type == Item.Type.Potion){
                RemoveModifiers(item);
                ClearSlot(PotionSlot);
                potionEquipped = false;
            }
        }
        updateText();
        
    }


	public void FillSlot (Item slot, Item newItem)
	{
        slot.icon = newItem.icon;
        slot.type = newItem.type;
        slot.damageModifier = newItem.damageModifier;
        slot.maxHealthModifier = newItem.maxHealthModifier;
        slot.armorModifier = newItem.armorModifier;
        Image icon = slot.gameObject.transform.GetChild(0).GetComponent<Image>();
        icon.enabled = true;
        icon.sprite = newItem.icon;

	}

    public void ClearSlot(Item slot)
	{
        slot.icon = null;
        slot.type = Item.Type.Sword;
        slot.damageModifier = 0;
        slot.maxHealthModifier = 0;
        slot.armorModifier = 0;
        Image icon = slot.gameObject.transform.GetChild(0).GetComponent<Image>();
        icon.enabled = false;
        icon.sprite = null;
	}

  public void AddModifiers(Item item)
  {
      stats.damage.AddModifier(item.damageModifier);
      stats.maxHealth.AddModifier(item.maxHealthModifier);
      stats.armor.AddModifier(item.armorModifier);
  }

  public void RemoveModifiers(Item item)
  {
      stats.damage.RemoveModifier(item.damageModifier);
      stats.maxHealth.RemoveModifier(item.maxHealthModifier);
      stats.armor.RemoveModifier(item.armorModifier);
  }
  
  public void updateText() {
      statsDisplay.text = string.Format("Max Health: {0}\nDamage: {1}\nArmor: {2}", stats.maxHealth.GetValue(), stats.damage.GetValue(), stats.armor.GetValue());
  } 



}