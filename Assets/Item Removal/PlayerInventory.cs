using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour {
    public KeyCode inventoryKey = KeyCode.I;
    public Sprite defaultSwordIcon;
    public Sprite defaultRingIcon;
    public static bool InventoryIsActive = false;

    private GameObject defaultSwordIcon_;
    private GameObject defaultRingIcon_;
    private GameObject inventoryUI;	
	private Transform storage;	
    private bool swordEquipped = false;
    private bool ringEquipped = false;
    private int space = 24;	
    private int slotsOccupied = 0;    
    private Transform inv;
    private Item SwordSlot;
    private Item RingSlot;
    private CharacterStats stats;
    private Text statsDisplay;
    private bool swordIsEquipped() {return swordEquipped;}
    private bool ringIsEquipped() {return ringEquipped;}
    private Vector3 noCollisionsArea; // the location where an item was discarded most recently (want to prevent dropped items from being immediately collected, since they are dropped at the player's position)
    private bool collisionsOn = true; 
    private const int COLLISION_DIST = 1; // an (arbitrary) distance that the player has to move away from the dropped item before item collection is re-enabled 

	private void Start ()
	{
        initializeVariables();      
        initializeButtons();
        inventoryUI.SetActive(false);
        updateText();
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    private void initializeVariables()
    {
        inv = GameObject.Find("CanvasScreenSpace").transform.Find("Inventory");
        inventoryUI = inv.gameObject;
        statsDisplay = inv.transform.Find("Stats").GetComponent<Text>();
        Transform weapons = inv.transform.Find("Weapons");
        SwordSlot = weapons.transform.Find("SwordSlot").GetComponent<Item>();
        RingSlot = weapons.transform.Find("RingSlot").GetComponent<Item>();
        stats = GameObject.Find("Player(Clone)").GetComponent<CharacterStats>();
        defaultSwordIcon_ = weapons.transform.Find("SwordSlot").Find("UseButton").Find("Icon").gameObject;
        defaultRingIcon_ = weapons.transform.Find("RingSlot").Find("UseButton").Find("Icon").gameObject;
        defaultSwordIcon_.GetComponent<Image>().sprite = defaultSwordIcon;
        defaultRingIcon_.GetComponent<Image>().sprite = defaultRingIcon;
    }

	private void Update ()
	{
		if (Input.GetKeyDown(inventoryKey) && !PauseMenu.IsPaused)
		{
            toggleInventory();
		}
        checkCollisions(); // check whether the player has moved far enough away from the last dropped item that we can re-enable collisions without immediately picking it up
	}

    private void toggleInventory() {
        // pause the game and display cursor
        Time.timeScale = (Time.timeScale == 0) ? 1 : 0; 
        Cursor.visible = !Cursor.visible;    
        Cursor.lockState = CursorLockMode.Confined;       

        // toggle UI
        inventoryUI.SetActive(!inventoryUI.activeSelf);
        InventoryIsActive = inventoryUI.activeSelf;

        // mute gameplay sounds
        gameObject.GetComponent<PlayerAudioController>().togglePause();
        gameObject.GetComponent<PlayerAudioController>().stopSounds(); 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!collisionsOn) {
            return; // prevents the player from picking up an item that has just been dropped
        }
        if(collision.collider.gameObject.tag == "Item"){
            Item item = collision.collider.gameObject.GetComponent<Item>();       
            if (slotsOccupied < space) {
                AddToInv(item);
                collision.collider.gameObject.SetActive(false);
                slotsOccupied++;
                gameObject.GetComponent<PlayerAudioController>().pickItem();
            }
        }
    }
    
    // re-enabled item collection
    private void checkCollisions(){ 
        // check that the noCollisionsArea variable has actually been set to something, then 
        // check the distance between the player and the last dropped item     
        if((noCollisionsArea != null) && (Vector3.Distance(gameObject.transform.position, noCollisionsArea) > COLLISION_DIST)){
            collisionsOn = true;
        }  
    }

    private void AddToInv(Item item)
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

    private void initializeButtons() {
        Transform storage = inv.transform.Find("Storage");
		Item[] slots = storage.GetComponentsInChildren<Item>();
		for (int i = 0; i < slots.Length; i++)
		{
            slots[i].startingObject = gameObject; // set a default startingObject, so that we can recognize which slots are empty
            Button button = slots[i].transform.Find("UseButton").GetComponent<Button>();
            button.onClick.AddListener(() => clickSlot(button.gameObject));
		}
        Button swordButton = SwordSlot.transform.Find("UseButton").GetComponent<Button>();
        swordButton.onClick.AddListener(() => clickSlot(swordButton.gameObject));
        Button ringButton = RingSlot.transform.Find("UseButton").GetComponent<Button>();
        ringButton.onClick.AddListener(() => clickSlot(ringButton.gameObject));

    }

    // Removes item from inventory
    public void Discard(Item item){
        GameObject startingObject = new GameObject(); // this may be unnecessary 
        startingObject = item.startingObject; 
        if(startingObject != gameObject){ // make sure it isn't discarding an empty slot
            startingObject.SetActive(true); // revive the item's original gameObject (eg. potion)
            startingObject.transform.position = gameObject.transform.position; // place it at the player's position
            collisionsOn = false; // prevent the gameObject from immediately being picked up
            noCollisionsArea = gameObject.transform.position; // prevent collisions in this area
            ClearSlot(item); // clear the inventory slot
        }        
    }

    private void clickSlot (GameObject button) {    
        Item item = button.transform.parent.gameObject.GetComponent<Item>();
        if(item.icon == null) { return; }
        if(button.transform.parent.parent.name == "Storage"){
            equipItem(item);
        }
        else {
            unequipItem(item);
        }
        updateText();        
    }

    private void equipItem(Item item){
        if((item.type == Item.Type.Sword) && (!swordIsEquipped())){
            AddModifiers(item);
            FillSlot(SwordSlot, item);
            ClearSlot(item);
            swordEquipped = true;   
            defaultSwordIcon_.SetActive(false);
            slotsOccupied--;
        }
        if((item.type == Item.Type.Ring) && (!ringIsEquipped())){
            AddModifiers(item);
            FillSlot(RingSlot, item);
            ClearSlot(item);
            ringEquipped = true;
            defaultRingIcon_.SetActive(false);
            slotsOccupied--;
        }
        if((item.type == Item.Type.Potion) && (item.potionEffect == Item.Effect.refillHealth)){
            stats.Heal(item.effectValue);
            ClearSlot(item);
            slotsOccupied--;
        }
    }

    private void unequipItem(Item item){
        AddToInv(item);
        RemoveModifiers(item);
        if(item.type == Item.Type.Sword){
            ClearSlot(SwordSlot);
            swordEquipped = false;
            defaultSwordIcon_.SetActive(true);
        }
        if(item.type == Item.Type.Ring){
            ClearSlot(RingSlot);
            ringEquipped = false;
            defaultRingIcon_.SetActive(true);

        }
    }

	private void FillSlot (Item slot, Item newItem)
	{
        slot.icon = newItem.icon;
        slot.type = newItem.type;
        slot.damageModifier = newItem.damageModifier;
        slot.maxHealthModifier = newItem.maxHealthModifier;
        slot.armorModifier = newItem.armorModifier;
        slot.potionEffect = newItem.potionEffect;
        slot.effectValue = newItem.effectValue;      
        slot.startingObject = newItem.startingObject;  
        Image icon = slot.gameObject.transform.GetChild(0).GetComponent<Image>();
        icon.enabled = true;
        icon.sprite = newItem.icon;
	}

    private void ClearSlot(Item slot)
	{
        slot.icon = null;
        slot.type = Item.Type.Sword;
        slot.damageModifier = 0;
        slot.maxHealthModifier = 0;
        slot.armorModifier = 0;
        slot.startingObject = gameObject; // default value        
        Image icon = slot.gameObject.transform.GetChild(0).GetComponent<Image>();
        icon.enabled = false;
        icon.sprite = null;
	}

    private void AddModifiers(Item item)
    {
        stats.damage.AddModifier(item.damageModifier);
        stats.maxHealth.AddModifier(item.maxHealthModifier);
        stats.armor.AddModifier(item.armorModifier);
    }

    private void RemoveModifiers(Item item)
    {
        stats.damage.RemoveModifier(item.damageModifier);
        stats.maxHealth.RemoveModifier(item.maxHealthModifier);
        stats.armor.RemoveModifier(item.armorModifier);
    }

    private void updateText() {
    statsDisplay.text = string.Format("Max Health: {0}\nDamage: {1}\nArmor: {2}", 
        stats.maxHealth.GetValue(), stats.damage.GetValue(), stats.armor.GetValue());
    } 

}