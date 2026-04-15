using UnityEngine;

public class CollectInteractable : MonoBehaviour, Interactable
{
    [SerializeField] string interactableName = "Item";
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
    public void Interact(GameObject player) {
        Debug.Log("Collected" + interactableName);
        if (destroyOnInteract) 
            makedForDestruction = true;
        player.GetComponent<CharacterController>().collectionAnimator.SetTrigger(interactableName);
        player.GetComponent<CharacterController>().inventoryManager.AddItem(collecableSettings);
    }
}