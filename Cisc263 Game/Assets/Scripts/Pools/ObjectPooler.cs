using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour, Pooler
{
    [SerializeField] private GameObject prefabObject;
    [SerializeField] private int poolDepth;
    [SerializeField] private bool canGrow = true;

    private readonly List<GameObject> pool = new List<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < poolDepth; i++)
        {
            GameObject pooledObject = Instantiate(prefabObject);
            pooledObject.SetActive(false);
            pool.Add(pooledObject);
        }
    }

    public GameObject GetObject()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (pool[i].activeInHierarchy == false)
                return pool[i];
        }

        if (canGrow == true)
        {
            GameObject pooledObject = Instantiate(prefabObject);
            pooledObject.SetActive(false);
            pool.Add(pooledObject);

            return pooledObject;
        }
        else
            return null;
    }

    public void ResetPool()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (pool[i].activeInHierarchy)  // need to go through everything for scent pools as random ones could be inactive at any time, can't just break when hit an inactive item
            {
                pool[i].SetActive(false);
            }
        }
    }
}
