using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {
    public List<string> sceneOrder = new List<string>();
    public int stage = 1; // tutorial stage
    [SerializeField] GameObject player, bee;

    private void Awake() {
        player = GameObject.FindWithTag("Player");
    }

    private void LateUpdate() {
        try {
            if (bee == null)
                bee = GameObject.FindWithTag("Bee");
        }
        catch {}
    }

    public void BeeTaxiChange() {
        stage++;
        bee.GetComponent<Animator>().SetTrigger("Bee Trigger");
        StartCoroutine(BeePause());
    }
    IEnumerator BeePause() {
        Debug.Log("Starting bee pause");
        CharacterController charControl = player.GetComponent<CharacterController>();
        charControl.loadingMode = true;
        Debug.Log(charControl + " load set true");
        yield return new WaitForSeconds(3.5f);

        Debug.Log("Delay passed");
        bee.GetComponent<BeeInteractable>().spr.enabled = false;
        Debug.Log("Disabled Spring");
        Destroy(bee);
        Debug.Log("Bee gone :(");
        yield return null;

        //Start loading new scene
        Debug.Log("Checking load for: " + stage + "/" + sceneOrder[stage]);
        if (sceneOrder[stage] == null || stage >= sceneOrder.Count)
            Debug.LogWarning("Invalid Scene to be loaded: " + sceneOrder[stage]);

        // Start loading scene asynchronously
        Debug.Log("Starting async load: " + sceneOrder[stage]);
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneOrder[stage]);
        while (!op.isDone) {
            Debug.Log("Progress: " + op.progress);
            yield return null;
        }
        Debug.Log("Scene loaded: " + sceneOrder[stage]);
        yield return null;

        charControl.SetupPlayer();
        charControl.loadingMode = false;
        GetComponent<PauseManager>().UpdateSound();
    }
}
