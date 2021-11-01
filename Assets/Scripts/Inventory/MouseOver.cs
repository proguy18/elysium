using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOver : MonoBehaviour
{
    // Start is called before the first frame update
    Item item;
    public GameObject player;
    private PlayerInventory inventory;
    private void Start() {
        item = gameObject.GetComponent<Item>();
        inventory = player.GetComponent<PlayerInventory>();
    }
    public void mousedOver() {
        Debug.Log("mousing over");
        inventory.triggerDelta(item);
    }
    public void unmousedOver(){
        inventory.untriggerDelta();
    }
}
