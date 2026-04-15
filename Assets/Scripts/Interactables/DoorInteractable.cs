using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorInteractable : MonoBehaviour, Interactable {
    [Tooltip("The name of the scene to swap to when interacted with")]
    [SerializeField] string targetScene;
    SceneChanger sceneChanger;

    void Awake() {
        Collider2D col;
        if (!TryGetComponent<Collider2D>(out col)) // if failed to get existing collider, add one
            col = gameObject.AddComponent<BoxCollider2D>();
        col.isTrigger = true;

        sceneChanger = GameObject.FindWithTag("Manager").GetComponent<SceneChanger>();
        Scene scene = SceneManager.GetSceneByName(targetScene);
        if (scene == null || scene.Equals(null)) Debug.LogError("Failed to find build scene: " + targetScene + "\n" + scene);
    }

    public void Interact(GameObject player) {
        try {
           sceneChanger.ChangeScene(targetScene);
        }
        catch (Exception e) {
            Debug.LogError($"Failed to change scene {targetScene} with error\n{e}");
        }
    }
}
