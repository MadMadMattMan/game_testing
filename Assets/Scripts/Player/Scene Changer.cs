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
        stage++;
        bee.GetComponent<Animator>().SetTrigger("Bee Trigger");
        StartCoroutine(BeePause());
    }
    IEnumerator BeePause() {
        yield return new WaitForSeconds(3.5f);
        Destroy(bee);
        player.transform.position = Vector3.zero;
        yield return SceneManager.LoadSceneAsync(sceneOrder[stage]);

        player.GetComponent<CharacterController>().SetupPlayer();
    }
}
