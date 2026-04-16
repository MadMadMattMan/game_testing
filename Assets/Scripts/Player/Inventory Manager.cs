using UnityEngine;
using UnityEngine.UI;

[System.Serializable] // Allows for sub-script inspector editing
public class InventoryManager {

    public CharacterController controller;
    public Collectable[] inventory = new Collectable[9];

    public void InitializeInventory(CharacterController controller) {
        this.controller = controller;
        for (int i = 0; i < inventory.Length; i++) 
            inventory.SetValue(Collectable.empty, i);
    }
    public Image[] inventorySlots;

    public bool AddItem(Collectable item) {
        for (int i = 0; i < 9; i++) {
            if (inventory[i].Equals(Collectable.empty)) { // add item to first available slot
                inventory.SetValue(item, i);
                ApplyItemEffects(item);
                inventorySlots[i].sprite = item.small; // set graphics of slot
                inventorySlots[i].color = new Color(255, 255, 255, 255);
                return true;
            }
        }        
        return false;
    }
    void ApplyItemEffects(Collectable item) {

    }
}
