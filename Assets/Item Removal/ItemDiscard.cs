using UnityEngine;
using UnityEngine.EventSystems;
    

// Attach this component to each UseButton in SampleScene (CanvasScreenSpace -> Inventory -> Storage -> InventorySlot)
// This is already done in the included copy of sampleScene
public class ItemDiscard : MonoBehaviour, IPointerClickHandler
{
    private PlayerInventory playerInv;
    private Item item;

    private void Update(){
        item = gameObject.GetComponentInParent<Item>(); // the item slot 

        // wait for player to spawn, then find it
        if(!playerInv && GameObject.Find("Player(Clone)").GetComponent<PlayerInventory>()){
            playerInv = GameObject.Find("Player(Clone)").GetComponent<PlayerInventory>(); 
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right){ // when slot icon is clicked 
            playerInv.Discard(item); // Attempt to discard whatever is in the slot
        }
    }
}
