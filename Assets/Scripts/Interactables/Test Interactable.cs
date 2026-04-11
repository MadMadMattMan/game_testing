using UnityEngine;

public class TestInteractable : MonoBehaviour, Interactable
{
    [SerializeField] string debugMessage = "Test Interactable was interacted with";
    [SerializeField] bool destroyOnInteract = true;
    bool makedForDestruction = false;
    [SerializeField] Collectable collecableSettings;

    // check and force add a trigger to this object on awake
    void Awake() {
        Collider2D col;
        if (!TryGetComponent<Collider2D>(out col)) // if failed to get existing collider, add one
            col = gameObject.AddComponent<BoxCollider2D>();
        col.isTrigger = true;
    }
    // last call before end of frame, helps avoid referencing errors if destruction happens here
    void LateUpdate() {
        if (makedForDestruction) 
            Destroy(gameObject);
    }
    public void Interact(InventoryManager inventory) {
        Debug.Log(debugMessage);
        if (destroyOnInteract) 
            makedForDestruction = true;
        inventory.AddItem(collecableSettings);
    }
}