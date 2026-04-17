using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorInteractable : MonoBehaviour, Interactable
{

    [SerializeField] GameObject fadeAnimation;
    GameObject managers, player;
    bool interacting = false;

    void Awake() {
        managers = GameObject.FindWithTag("Manager");
        player = GameObject.FindWithTag("Player");
    }

    public void Interact(GameObject player)
    {
        if (!interacting) {
            StartCoroutine(EndAnimationPause());
            interacting = true;
        }
    }

    IEnumerator EndAnimationPause() {
        fadeAnimation.gameObject.SetActive(true);
        yield return new WaitForSeconds(10.0f);

        player.GetComponent<CharacterController>().loadingMode = true;
        Destroy(player);
        Destroy(managers.GetComponent<PauseManager>().PauseMenu);
        Destroy(managers);
        SceneManager.LoadScene("Main Scene"); // reset game
    }
}