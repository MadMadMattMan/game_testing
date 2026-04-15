using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {
    public List<string> sceneOrder = new List<string>();
    public int stage = 0; // tutorial stage
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
        bee.GetComponent<Animator>().SetTrigger("Bee Trigger");
        StartCoroutine(BeePause());
    }
    IEnumerator BeePause() {
        stage++;
        yield return new WaitForSeconds(5.0f);

        SceneManager.LoadScene(sceneOrder[stage]);
    }
}
