using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class PauseManager : MonoBehaviour {
    public GameObject PauseMenu, CreditsPanel; // The pause Menu Canvas
    [SerializeField] bool paused = false;
    bool pressed = false; // tracker to avoid spam toggle
    bool credits = false;

    InputAction pauseAction;

    [SerializeField] List<AudioSource> audioInScene = new List<AudioSource>();
    public float volume = 1.0f;

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
        audioInScene.Clear();
        audioInScene.Add(GameObject.FindWithTag("Player").GetComponent<AudioSource>());
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Audio")) {
            AudioSource a = go.GetComponent<AudioSource>();
            audioInScene.Add(a);
            a.volume = volume;
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

    public void UpdateVolume(Single s) {
        volume = s;
        foreach (AudioSource a in audioInScene)
            a.volume = volume;
    }
}
