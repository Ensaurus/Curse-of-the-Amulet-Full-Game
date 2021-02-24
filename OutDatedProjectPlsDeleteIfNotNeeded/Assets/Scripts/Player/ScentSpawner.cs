using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScentSpawner : MonoBehaviour
{
    private bool stationary;
    private bool staticSpawning;
    private Vector2 updatePos;
    private Vector2 spawnPos;
    private Vector2 nextSpawnPos;
    private ObjectPooler scentPool;
    private Transform myTransform;
    [SerializeField] private float validDistance; // distance moved in unity units in last 4 secs for not to be stationary
    [SerializeField] private float stinkiness; // pungence of scent nodes
    [SerializeField] private float spawnRate; // time in seconds between spawns

    // Start is called before the first frame update
    void Start()
    {
        myTransform = this.transform;
        updatePos = myTransform.position;
        spawnPos = myTransform.position;
        nextSpawnPos = myTransform.position;
        stationary = false;
        staticSpawning = false;
        scentPool = GetComponent<ScentPool>();

        InvokeRepeating("UpdateLocation", 4, 4);    // check if stationary every 4 secs
        InvokeRepeating("MovingSpawn", 1, spawnRate);   // spawn a scent every half second
        InvokeRepeating("StaticSpawn", 2, spawnRate);
    }

    void UpdateLocation()
    {
        Vector2 currentPos = myTransform.position;
        float distanceMoved = (currentPos - updatePos).magnitude;

        if (distanceMoved <= validDistance)
        {
            stationary = true;
        }
        else
        {
            stationary = false;
        }

        updatePos = currentPos;
    }

    private void MovingSpawn()
    {
        if (!stationary)
        {
            spawnPos = nextSpawnPos;

            

            GameObject node = scentPool.GetAvailableObject();
            node.transform.position = spawnPos;
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
            GameObject node = scentPool.GetAvailableObject();
            node.transform.position = myTransform.position;
            node.transform.localScale = new Vector3(radius, radius, radius);
            node.SetActive(true);

            Scent scent = node.GetComponent<Scent>();
            scent.next = myTransform.position;
            scent.pungence = stinkiness;
            radius += 0.5f;
            yield return new WaitForSeconds(0.5f);
        }

        
        staticSpawning = false;
    }
}
