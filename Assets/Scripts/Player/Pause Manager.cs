using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour {
    public GameObject PauseMenu, CreditsPanel; // The pause Menu Canvas
    [SerializeField] bool paused = false;
    bool pressed = false; // tracker to avoid spam toggle
    bool credits = false;

    InputAction pauseAction;

    void Awake() {
        if (GameObject.FindGameObjectsWithTag("Manager").Length > 1) {
            Destroy(PauseMenu);
            Destroy(gameObject);
        }
    }

    void Start() {
        if (PauseMenu != null) {
            // Marks the pause objects as keep when changing scenes
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(PauseMenu);
            pauseAction = InputSystem.actions.FindAction("Pause");
        }
    }

    void Update() {
        if (PauseMenu != null) {
            if (pauseAction.IsPressed() && !pressed)
            {
                TogglePause();
                pressed = true;
            }
            else if (!pauseAction.IsPressed() && pressed)
                pressed = false;
        }
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
            if (credits) Credits();
        }
    }

    public void Credits() {
        credits = !credits;
        CreditsPanel.SetActive(credits);
    }

    public void Quit() {
        Application.Quit();
    }

    public void StartGame() {
        SceneManager.LoadScene("Main Scene");
    }
}
