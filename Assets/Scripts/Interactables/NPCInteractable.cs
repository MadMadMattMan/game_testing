using UnityEngine;

public class NPCInteractable : MonoBehaviour, Interactable {
   
    NPCManager manager;

    void Awake() {
        manager = GetComponent<NPCManager>();
    }

    public void Interact(GameObject player) {
        manager.Interact();
    }
}
