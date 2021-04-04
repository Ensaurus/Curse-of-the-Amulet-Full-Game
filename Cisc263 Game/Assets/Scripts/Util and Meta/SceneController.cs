using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : Singleton<SceneController>
{
    public float currentLevel;
    public int numEnemies;
    public int numChargingStations;
    public int numPaths;
    public int numFlames;

    // width and height of current level map, map extends from (0,0,0) to levelDimensions
    public Vector2 levelDimensions;

    // specifies increase in quanity of different items in subsequent levels
    public Vector2 levelSizeIncrease;
    public int enemyIncrease;
    public int chargingStationIncrease;
    public int pathIncrease;
    public int flamesIncrease;

    private void Start() {
        currentLevel = 1;

        SpawnLevel();
        EventManager.Instance.LevelCompleted.AddListener(ChangeLevel);
	}

    private void SpawnLevel()
    {
        // done in seperate scipt so this doesnt need to be cluttered with fields for object pools
        SpawnManager.Instance.LevelSpawner(numEnemies, numPaths, numChargingStations, numFlames, levelDimensions);
    }

    private void UnloadLevel()
    {
        SpawnManager.Instance.LevelUnloader();
    }

    private void ChangeLevel()
    {
        currentLevel++;       
        levelDimensions += levelSizeIncrease;
        numEnemies += enemyIncrease;
        numChargingStations += chargingStationIncrease;
        numPaths += pathIncrease;
        UIManager.Instance.LevelTransitionText();

        UnloadLevel();
        SpawnLevel();
    }
}