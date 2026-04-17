using System.Collections;
using UnityEngine;

public class CollectInteractable : MonoBehaviour, Interactable
{
    [SerializeField] bool destroyOnInteract = true;
    bool makedForDestruction = false;

    [SerializeField] Collectable collecableSettings;
    SpriteRenderer smallItem;
    UnityEngine.UI.Image largeItem;
    Animator amr;
    InventoryManager inventoryManager;

    // check and force add a trigger to this object on awake
    void Awake() {
        SpriteRenderer renderer = GetComponentInChildren<SpriteRenderer>();
        Collider2D col;
        if (!renderer.gameObject.TryGetComponent<Collider2D>(out col))
            col = renderer.gameObject.AddComponent<BoxCollider2D>();
        col.isTrigger = true;

        amr = GetComponent<Animator>();

        smallItem = GetComponentInChildren<SpriteRenderer>();
        smallItem.sprite = collecableSettings.ground;

        largeItem = GetComponentInChildren<UnityEngine.UI.Image>();
        largeItem.sprite = collecableSettings.big;
    }
    // last call before end of frame, helps avoid referencing errors if destruction happens here
    void LateUpdate() {
        if (makedForDestruction) 
            Destroy(gameObject);
    }
    public void Interact(GameObject player) {
        Debug.Log("Collected: " + collecableSettings.name);
        amr.SetTrigger("Interacted");
        inventoryManager = player.GetComponent<CharacterController>().inventoryManager;
        smallItem.sprite = null;
        StartCoroutine(CollectDelayed());
    }

    public IEnumerator CollectDelayed() {
        amr.SetTrigger("Collected");
        yield return new WaitForSeconds(6.0f);
        inventoryManager.AddItem(collecableSettings);
        makedForDestruction = destroyOnInteract;
    }
}