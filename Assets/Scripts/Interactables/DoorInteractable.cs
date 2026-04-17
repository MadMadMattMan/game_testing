using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorInteractable : MonoBehaviour, Interactable
{

    [SerializeField] Animator fadeAnimation;
    GameObject managers, player;

    public bool interacting = false;

    void Awake() {
        managers = GameObject.FindWithTag("Manager");
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if (interacting) { 
            interacting = false;
            Interact(player);
        }
    }

    public void Interact(GameObject player)
    {
        if (!interacting) {
            StartCoroutine(EndAnimationPause());
        }
    }

    IEnumerator EndAnimationPause() {
        fadeAnimation.gameObject.SetActive(true);
        fadeAnimation.SetTrigger("fade out");
        yield return new WaitForSeconds(6.0f);

        Destroy(player);
        Destroy(managers.GetComponent<PauseManager>().PauseMenu);
        Destroy(managers);
        SceneManager.LoadScene("Main Scene"); // reset game
    }
}