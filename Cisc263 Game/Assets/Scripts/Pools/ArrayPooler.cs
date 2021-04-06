using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayPooler : MonoBehaviour, Pooler
{
	public bool empty; // for powerups, if all get collected and no reusable ones available
	[SerializeField] private GameObject[] objectsToPool;
	[SerializeField] private int poolDepth;
	[SerializeField] protected bool canGrow = true;

	public List<GameObject>[] poolArray;  // array of the different pool lists, of length objectsToPool.Length  readonly (unless it's for powerups, oopsies)

	private void Awake()
	{
		poolArray = new List<GameObject>[this.objectsToPool.Length];

		for (int i = 0; i < poolArray.Length; i++)
		{
			poolArray[i] = new List<GameObject>();  // check back later, might not need this line

			for (int j = 0; j < poolDepth; j++)
			{
				GameObject pooledObject = Instantiate(objectsToPool[i]);
				pooledObject.SetActive(false);
				poolArray[i].Add(pooledObject);
			}
		}
	}

	public virtual GameObject GetObject()
	{
		int listArrayIndex = Random.Range(0, poolArray.Length);     // get a random pool list of game objects from the poolArray 

		for (int listIndex = 0; listIndex < poolArray[listArrayIndex].Count; listIndex++)
		{
			if (poolArray[listArrayIndex][listIndex].activeInHierarchy == false)
			{   // if the list at the listArrayIndex has a gameobject at one of its indexes but isn't active
				return poolArray[listArrayIndex][listIndex];
			}
		}

		if (canGrow == true)
		{
			GameObject pooledObject = Instantiate(poolArray[listArrayIndex][0]);    // just took the first gameobject of the randomly chosen list
			pooledObject.SetActive(false);
			poolArray[listArrayIndex].Add(pooledObject);
			return pooledObject;
		}
		else
		{
			return null;
		}
	}

	// look through all nested pools and if any items are active from the pool, deactivate them
	public void ResetPool()
    {
		for (int i = 0; i < poolArray.Length; i++)
        {
			for (int j = 0; j < poolArray[i].Count; j++)
            {
				if (poolArray[i][j].activeInHierarchy == true)
				{
					poolArray[i][j].SetActive(false);
				}
                else
                {
					break;
                }
			}
        }
    }
}