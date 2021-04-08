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
    public int portalChargeRequired;

    // width and height of current level map, map extends from (0,0,0) to levelDimensions
    public Vector2 levelDimensions;

    // specifies increase in quanity of different items in subsequent levels
    public Vector2 levelSizeIncrease;
    public int enemyIncrease;
    public int chargingStationIncrease;
    public int pathIncrease;
    public int flamesIncrease;
    public int portalChargeIncrease;

    private void Start() {
        currentLevel = 1;
        Amulet.Instance.maxCharge = portalChargeRequired + 10;

        SpawnLevel();
        EventManager.Instance.FadeComplete.Invoke(); // on first level spawn, invoke fadeComplete so enemyAI unfreezes
        EventManager.Instance.LevelCompleted.AddListener(ChangeLevel);
	}

    private void SpawnLevel()
    {
        // done in seperate scipt so this doesnt need to be cluttered with fields for object pools
        SpawnManager.Instance.LevelSpawner(numEnemies, numPaths, numChargingStations, numFlames, levelDimensions, portalChargeRequired);
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
        portalChargeRequired += portalChargeIncrease;
        Amulet.Instance.maxCharge = portalChargeRequired + 10;

        UnloadLevel();
        SpawnLevel();
        UIManager.Instance.LevelTransitionText();
    }
}