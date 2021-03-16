using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    private ObjectPooler enemies;
    private ArrayPooler powerUps;
    private ArrayPooler obstacles;
    private ObjectPooler exit;
    private ObjectPooler chargingStations;
    [SerializeField] private GameObject map;
    private Transform mapTransform;
    [SerializeField] private GameObject player;
    [SerializeField] private Vector2 playerSpawnPos;
    [SerializeField] private Vector2 spaceBuffer; // required distance certain objects must be from player when level starts

    private ScentSpawner scentSpawner;
    private ScentPool scentPool;

    protected override void Awake()
    {
        base.Awake();
        enemies = gameObject.GetComponent<EnemyPool>();
        chargingStations = gameObject.GetComponent<ChargingStationPool>();
        powerUps = gameObject.GetComponent<PowerUpPool>();
        exit = gameObject.GetComponent<ExitPool>();
        obstacles = gameObject.GetComponent<ObstaclesPool>();

        scentSpawner = player.GetComponent<ScentSpawner>();
        scentPool = player.GetComponent<ScentPool>();

        mapTransform = map.transform;
    }

    private void Start()
    {
        // add listener for when new level done fading in to activate scent spawning
        EventManager.Instance.FadeComplete.AddListener(ActivateScentSpawning);
    }


    #region level spawner

    public void LevelSpawner(int enemyNum, int powerUpNum, int chargingStationNum, int obstacleNum, Vector2 mapDimensions)
    {
        mapTransform.localScale = new Vector3 (mapDimensions.x, mapDimensions.y, 1);

        // place player at middle of map (activate scent spawning once ui faded out)
        playerSpawnPos = mapDimensions/2;
        player.transform.position = playerSpawnPos;



        // spawn enemies
        for (int i = 0; i < enemyNum; i++)
        {
            SpawnRandomPos(enemies, mapDimensions, false);
        }
        // spawn powerups
        for (int i = 0; i < powerUpNum; i++)
        {
            SpawnRandomPos(powerUps, mapDimensions, true);
        }
        // spawn charging stations
        for (int i = 0; i < chargingStationNum; i++)
        {
            SpawnRandomPos(chargingStations, mapDimensions, true);
        }
        // spawn obstacles
        for (int i = 0; i < obstacleNum; i++)
        {
            SpawnRandomPos(obstacles, mapDimensions, true);
        }
        // place exit
        SpawnRandomPos(exit, mapDimensions, false);
    }

    private void SpawnRandomPos(Pooler pool, Vector2 dimensions, bool canBeNearPlayer)
    {
        GameObject obj = pool.GetObject();
        Transform objTransform = obj.transform;
        float xPos = Random.Range(0, dimensions.x);
        float yPos = Random.Range(0, dimensions.y);

        // if this item shouldn't spawn near player, recalculate until it is at least SceneController.spaceBuffer away
        if (!canBeNearPlayer){
            while (xPos >= (playerSpawnPos.x - spaceBuffer.x) && xPos <= (playerSpawnPos.x + spaceBuffer.x))
            {
                xPos = Random.Range(0, dimensions.x);
            }
            while (yPos >= (playerSpawnPos.y - spaceBuffer.y) && yPos <= (playerSpawnPos.y + spaceBuffer.y))
            {
                yPos = Random.Range(0, dimensions.y);
            }
        }
        objTransform.position = new Vector2(xPos, yPos);

        obj.SetActive(true);
    }


    private void ActivateScentSpawning()
    {
        scentSpawner.enabled = true;
    }
    #endregion

    public void LevelUnloader()
    {
        enemies.ResetPool();
        powerUps.ResetPool();
        chargingStations.ResetPool();
        obstacles.ResetPool();
        exit.ResetPool();
        // disable scent spawning and remove lingering scent nodes
        scentSpawner.enabled = false;
        scentSpawner.CancelInvoke();
        scentPool.ResetPool();
    }
}
