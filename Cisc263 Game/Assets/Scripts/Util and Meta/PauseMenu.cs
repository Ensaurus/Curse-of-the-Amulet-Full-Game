using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject ControlsUI;
    public GameObject LanternText;
    public GameObject AmultetText;
    public bool isPaused;
    private bool unpausable;

    private void Start()
    {
        EventManager.Instance.Death.AddListener(GameOver);
        EventManager.Instance.AttemptedExitWithEnoughCharge.AddListener(WaitForPlayer);
        EventManager.Instance.PortalNotTaken.AddListener(EnablePausing);
        EventManager.Instance.LevelCompleted.AddListener(EnablePausing);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !unpausable)
        {
            TogglePause();
        }
    }


    private void TogglePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }
    private void GameOver()
    {
        unpausable = true;
    }

    private void Resume(){
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    void Pause(){
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
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

    private void WaitForPlayer()
    {
        unpausable = true;
        isPaused = true;
        Time.timeScale = 0;
    }

    private void EnablePausing()
    {
        unpausable = false;
        isPaused = false;
        Time.timeScale = 1;
    }
}
