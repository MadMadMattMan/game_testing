using UnityEngine;

public class BeeInteractable : MonoBehaviour, Interactable {

    GameObject player;
    SceneChanger sceneChanger;
    
    Collider2D col;
    SpringJoint2D spr;

    void Awake() {
        if (!TryGetComponent<Collider2D>(out col)) // if failed to get existing collider, add one
            col = gameObject.AddComponent<BoxCollider2D>();
        col.isTrigger = true;

        player = GameObject.FindWithTag("Player");

        if (!TryGetComponent<SpringJoint2D>(out spr)) // if failed to get existing spring, add one
            spr = gameObject.AddComponent<SpringJoint2D>();
        spr.connectedBody = player.GetComponent<Rigidbody2D>();
        spr.distance = 0.005f;
        spr.dampingRatio = 0.25f;
        spr.frequency = 0f;
        spr.anchor = new Vector2(0.5f, -1f);

        sceneChanger = GameObject.FindWithTag("Manager").GetComponent<SceneChanger>();
    }

    public void Interact(GameObject player) {
        sceneChanger.BeeTaxiChange();
    }
}
