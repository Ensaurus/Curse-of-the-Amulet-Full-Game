using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : Singleton<SceneManager>
{
    public float currentLevel;
    public int numEnemies;
    public int numChargingStations;
    public int numPowerUps;
    public int numObstacles;

    // width and height of current level map, map extends from (0,0,0) to levelDimensions
    public float levelHeight;
    public float levelWidth;	
	private Vector2 levelDimensions;

    // specifies increase in quanity of different items in subsequent levels
    public Vector2 levelSizeIncrease;
    public int enemyIncrease;
    public int chargingStationIncrease;
    public int powerUpsIncrease;
    public int obstaclesIncrease;

    private void Start() {
        levelDimensions.Set(levelWidth, levelHeight);
        currentLevel = 1;

        SpawnLevel();
        EventManager.Instance.LevelCompleted.AddListener(ChangeLevel);
	}

    private void SpawnLevel()
    {
        // done in seperate scipt so this doesnt need to be cluttered with fields for object pools
        SpawnManager.Instance.LevelSpawner(numEnemies, numPowerUps, numChargingStations, numObstacles, levelDimensions);
    }

    private void UnloadLevel()
    {
        SpawnManager.Instance.LevelUnloader();
    }

    private void ChangeLevel()
    {
        UIManager.Instance.LevelTransitionText();
        // replace later, just using TogglePause for video deliverable
        GameManager.Instance.TogglePause();

        levelDimensions += levelSizeIncrease;
        numEnemies += enemyIncrease;
        numChargingStations += chargingStationIncrease;
        numPowerUps += powerUpsIncrease;
        numObstacles += obstaclesIncrease;

        UnloadLevel();
        SpawnLevel();
    }
}