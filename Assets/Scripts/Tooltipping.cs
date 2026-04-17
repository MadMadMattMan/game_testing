using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tooltipping : MonoBehaviour
{
    [SerializeField] GameObject talk, sprint, interact;
    int state = 0;
    SceneChanger changer;
    InputAction e, shift;

    void Awake() {
        talk.SetActive(true);
        sprint.SetActive(false);
        interact.SetActive(false);
        changer = GameObject.FindWithTag("Manager").GetComponent<SceneChanger>();
        e = InputSystem.actions.FindAction("Interact");
        shift = InputSystem.actions.FindAction("Sprint");
    }

    private void FixedUpdate() {
        // collect
        bool ep = false, sp = false;
        if (state != 1) ep = e.IsPressed();
        else sp = shift.IsPressed();

        // Switch
        if (state == 0 && ep) { // talking
            state++;
            talk.SetActive(false);
            sprint.SetActive(true);
        }
        else if (state == 1 && sp) { // sprinting
            state++;
            sprint.SetActive(false);
            interact.SetActive(true);
        }
        else if (state == 2 && ep) { // bee use
            state++;
            interact.SetActive(false);
            Destroy(gameObject);
        }
    }
}
