using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorInteractable : MonoBehaviour, Interactable
{

    [SerializeField] GameObject fadeAnimation, wakeUpButton;
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

    public void WakeUp() {
        player.GetComponent<CharacterController>().loadingMode = true;
        player.SetActive(false);
        Destroy(player);
        Destroy(managers.GetComponent<PauseManager>().PauseMenu);
        Destroy(managers);
        SceneManager.LoadScene("Main Scene"); // reset game
    }

    IEnumerator EndAnimationPause() {
        player.GetComponent<CharacterController>().loadingMode = true;
        fadeAnimation.SetActive(true);
        yield return new WaitForSeconds(6.0f);
        wakeUpButton.SetActive(true);
    }
}