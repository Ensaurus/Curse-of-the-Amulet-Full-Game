using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
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
    
    private void LevelSpawner()
    {

    }

    #endregion
}
