using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public enum GameState
    {
        PREGAME,
        RUNNING,
        PAUSED
    }

    private GameState currentGameState = GameState.PREGAME;
    public GameState CurrentGameState
    {
        get
        {
            return currentGameState;
        }
    }


    private void Start()
    {
        EventManager.Instance.Death.AddListener(GameOver);

        // take out later when main menu added
        StartGame();
    }

    private void Update()
    {
        if (CurrentGameState == GameState.PREGAME)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }




    // Load game scene at first level
    private void StartGame()
    {
        UpdateState(GameState.RUNNING);
    }

    void UpdateState(GameState state)
    {
        GameState previousGameState = currentGameState;
        currentGameState = state;

        switch (currentGameState)
        {
            case GameState.PREGAME:
                Time.timeScale = 1.0f;
                break;

            case GameState.RUNNING:
                Time.timeScale = 1.0f;
                break;

            case GameState.PAUSED:
                Time.timeScale = 0.0f;
                break;

            default:
                break;
        }
    }

    public void TogglePause()
    {
        UpdateState(currentGameState == GameState.RUNNING ? GameState.PAUSED : GameState.RUNNING);
    }

    // Pause game and display game over screen
    private void GameOver()
    {
        StartCoroutine(WaitForScare());
    }

    IEnumerator WaitForScare()
    {
        while (UIManager.Instance.isScaring)
        {
            yield return null;
        }

        UIManager.Instance.DisplayGameOver();
        Time.timeScale = 0.0f;
    }


}