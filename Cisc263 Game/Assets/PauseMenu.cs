using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject ControlsUI;
    public GameObject LanternText;
    public GameObject AmultetText;

    private void Start()
    {
        EventManager.Instance.GameStateChange.AddListener(TogglePause);
    }


    private void TogglePause(GameManager.GameState previousGameState, GameManager.GameState newGameState)
    {
        if (newGameState == GameManager.GameState.PAUSED)
        {
            Pause();
        }
        if (previousGameState == GameManager.GameState.PAUSED && newGameState != GameManager.GameState.PAUSED)
        {
            Resume();
        }
    }

    public void Resume(){
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause(){
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void QuitGame(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadControls(){
        LanternText.SetActive(false);
        AmultetText.SetActive(false);
        pauseMenuUI.SetActive(false);
        ControlsUI.SetActive(true);
    }
    public void Back(){
        LanternText.SetActive(true);
        AmultetText.SetActive(true);
        pauseMenuUI.SetActive(true);
        ControlsUI.SetActive(false);
    }
}
