using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {

    Dictionary<string, bool> isPlayable = new Dictionary<string, bool>();
    GameObject player;

    private void Awake() {
        player = GameObject.FindWithTag("Player");

        // Add scenes with their player state bools
        isPlayable.Add("Main Scene", true);
        isPlayable.Add("Basic House Interior", false);
    }

    public void ChangeScene(string scene) {
        if (isPlayable.ContainsKey(scene)) {
            SceneManager.LoadScene(scene);
            bool playable = isPlayable[scene];
            player.SetActive(playable);
        }
    }
}
