using UnityEngine;

public class BuildmodeManager : MonoBehaviour {
    // References
    GameObject Managers;
    private void Start() {
        Managers = GameObject.FindWithTag("Manager");
    }

    // Build inventory

    // Selecting item for placing



    // UI Buttons
    public void Exit(string targetScene) {
        Managers.GetComponent<SceneChanger>().ChangeScene(targetScene);
    }
}