using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScentSpawner : Singleton<ScentSpawner>
{
    private bool stationary;
    private bool staticSpawning;
    private Vector2 lastUpdatePos;
    private Vector2 spawnPos;
    private Vector2 nextSpawnPos;
    private ObjectPooler scentPool;
    private Transform myTransform;
    [SerializeField] private float updateTime; // time in seconds between location updates to see if player moved
    [SerializeField] private float validDistance; // distance moved in unity units in last 4 secs for not to be stationary
    public float stinkiness; // pungence of scent nodes
    [SerializeField] private float spawnRate; // time in seconds between spawns
    [SerializeField] private float growthRate; // how much radius of scent will grow while standing still


    // Start is called before the first frame update
    override protected void Awake()
    {
        base.Awake();
        myTransform = this.transform;
        scentPool = GetComponent<ScentPool>();
    }

    private void OnEnable()
    {
        lastUpdatePos = myTransform.position;
        spawnPos = myTransform.position;
        nextSpawnPos = myTransform.position;
        stationary = false;
        staticSpawning = false;
        InvokeRepeating("UpdateLocation", 4, updateTime);    // check if stationary every updateTime secs
        InvokeRepeating("MovingSpawn", 1, spawnRate);   // spawn a scent every spawnRate seconds
        InvokeRepeating("StaticSpawn", 2, spawnRate);
    }

    void UpdateLocation()
    {
        Vector2 currentPos = myTransform.position;
        float distanceMoved = (currentPos - lastUpdatePos).magnitude;

        if (distanceMoved <= validDistance)
        {
            stationary = true;
        }
        else
        {
            stationary = false;
        }

        lastUpdatePos = currentPos;
    }

    private void MovingSpawn()
    {
        if (!stationary)
        {
            spawnPos = nextSpawnPos;

            

            GameObject node = scentPool.GetObject();
            node.transform.position = spawnPos;
            // need to reset scale of ones grown while stationary
            node.transform.localScale = new Vector3(1, 1, 1);
            node.SetActive(true);

            nextSpawnPos = myTransform.position;
            Scent scent = node.GetComponent<Scent>();
            scent.next = nextSpawnPos;
            scent.pungence = stinkiness;

            // Debug.Log("Scent spawning at: " + spawnPos + "next: " + nextSpawnPos);
        }
    }

    private void StaticSpawn()
    {
        if (stationary && !staticSpawning)
        {
            StartCoroutine(CircleSpawner());
        }
    }

    IEnumerator CircleSpawner()
    {
        staticSpawning = true;
        float radius = 1;

        while (stationary)
        {
            GameObject node = scentPool.GetObject();
            node.transform.position = myTransform.position;
            node.transform.localScale = new Vector3(radius, radius, radius);
            node.SetActive(true);

            Scent scent = node.GetComponent<Scent>();
            scent.next = myTransform.position;
            scent.pungence = stinkiness;
            radius += growthRate;
            yield return new WaitForSeconds(spawnRate);
        }

        
        staticSpawning = false;
    }
}
