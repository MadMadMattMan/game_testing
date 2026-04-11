using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable] // Allows for sub-script inspector editing
public class InventoryManager {

    public Collectable[] inventory = new Collectable[9];
    public void InitializeInventory() {
        for (int i = 0; i < inventory.Length; i++) 
            inventory.SetValue(Collectable.empty, i);
    }
    public Image[] inventorySlots;

    public bool AddItem(Collectable item) {
        for (int i = 0; i < 9; i++) {
            if (inventory[i].Equals(Collectable.empty)) { // add item to first available slot
                inventory.SetValue(item, i);
                ApplyItemEffects(item);
                inventorySlots[i].sprite = item.sprite; // set graphics of slot
                inventorySlots[i].color = new Color(255, 255, 255, 255);
                return true;
            }
        }        
        return false;
    }
    void ApplyItemEffects(Collectable item) {

    }

    public bool RemoveItem(Collectable item) {
        for (int i = 0; i < 9; i++) {
            if (inventory[i].Equals(item)) {
                inventory.SetValue(Collectable.empty, i);
                RemoveItemEffects(item);
                inventorySlots[i].sprite = null; // set graphics of slot
                inventorySlots[i].color = new Color(255, 255, 255, 0);
                return true;
            }
        }
        return false;
    }
    void RemoveItemEffects(Collectable item) {

    }
}
