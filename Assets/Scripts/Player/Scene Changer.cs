using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {

    Dictionary<string, bool> isPlayable = new Dictionary<string, bool>();
    GameObject player;
    GameObject build;

    private void Awake() {
        player = GameObject.FindWithTag("Player");
        build = GameObject.FindWithTag("Build");

        // Add scenes with their player state bools
        isPlayable.Add("Main Scene", true);
        isPlayable.Add("Basic House Interior", false);
    }

    public void ChangeScene(string scene) {
        if (isPlayable.ContainsKey(scene)) {
            SceneManager.LoadScene(scene);
            bool playable = isPlayable[scene];
            if (playable)
                StartCoroutine(SetupDelayed());
            else
                player.SetActive(false);
        }
    }
    IEnumerator SetupDelayed()
    {
        yield return new WaitForSeconds(0.01f);
        player.SetActive(true);
        player.GetComponent<CharacterController>().SetupPlayer();
    }
}
