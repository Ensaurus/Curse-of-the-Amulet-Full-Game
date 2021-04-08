using System.Collections.Generic;
using UnityEngine;
public class PowerUpPool : ArrayPooler
{
	private List<int> deadSpaces = new List<int>();	// keeps record of which indexes are no longer valid
	private void Start()
    {
        EventManager.Instance.PowerUpCollected.AddListener(RemoveFromPool);
    }

    public void RemoveFromPool(GameObject pickedUp)
    {
        PowerUp script = pickedUp.GetComponent<PowerUp>();
        // remove powerup from pool if only single use
        if (script.isSingleUse())
        {
            for (int i = 0; i < poolArray.Length; i++)
            {
                if (poolArray[i].Contains(pickedUp)){
                    poolArray[i] = new List<GameObject>();
					deadSpaces.Add(i);
                }
            }
        }
    }

	public override GameObject GetObject()
	{
		if (deadSpaces.Count == poolArray.Length)
        {
			Debug.Log("no valid powerups in pool, all been used");
			empty = true;
			Debug.Log("empty: " + empty);
			return null;
        }
		int listArrayIndex = Random.Range(0, poolArray.Length);     // get a random pool list of game objects from the poolArray 
		// if index generated has been "removed" recalculate until find a valid index
		while (deadSpaces.Contains(listArrayIndex)){
			listArrayIndex = Random.Range(0, poolArray.Length);
		}

		for (int listIndex = 0; listIndex < poolArray[listArrayIndex].Count; listIndex++)
		{
			if (poolArray[listArrayIndex][listIndex].activeInHierarchy == false)
			{   // if the list at the listArrayIndex has a gameobject at one of its indexes but isn't active
				return poolArray[listArrayIndex][listIndex];
			}
		}

		return null;
	}
}
