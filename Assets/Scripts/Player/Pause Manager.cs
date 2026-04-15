using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour {
    public GameObject PauseMenu; // The pause Menu Canvas
    [SerializeField] bool paused = false;
    bool pressed = false; // tracker to avoid spam toggle

    InputAction pauseAction;

    void Awake() {
        if (GameObject.FindGameObjectsWithTag("Manager").Length > 1) {
            Destroy(PauseMenu);
            Destroy(gameObject);
        }
    }

    void Start() {
        // Marks the pause objects as keep when changing scenes
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(PauseMenu);
        pauseAction = InputSystem.actions.FindAction("Pause");
    }

    void Update() {
        if (pauseAction.IsPressed() && !pressed) {
            TogglePause();
            pressed = true;
        }
        else if (!pauseAction.IsPressed() && pressed)
            pressed = false;
    }


    public void TogglePause() {
        Debug.Log("Pause Toggled");
        paused = !paused;

        if (paused) {
            Time.timeScale = 0f;
            PauseMenu.SetActive(true);
        }
        else {
            Time.timeScale = 1f;
            PauseMenu.SetActive(false);
        }
    }

    public void Quit() {
        Application.Quit();
    }
}
