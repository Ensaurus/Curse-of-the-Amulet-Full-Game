using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    private ObjectPooler enemies;
    private ArrayPooler powerUps;
    private ArrayPooler obstacles;
    private ObjectPooler exits;
    private ObjectPooler amuletChargers;

    private void Start()
    {
        enemies = gameObject.GetComponent<EnemyPool>();
        powerUps = gameObject.GetComponent<PowerUpPool>();
    }

    #region level spawner
    
    public void LevelSpawner(int enemieNum, int powerUpNum, int chargingStationNum, int obstacleNum, Vector2 mapDimensions)
    {

    }

    #endregion

    public void LevelUnloader()
    {

    }
}
