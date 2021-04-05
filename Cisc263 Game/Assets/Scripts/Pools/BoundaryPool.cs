using UnityEngine;

public class BoundaryPool : ArrayPooler
{
    // need to put upper in pool[0], left in pool[1], lower in pool[2], right in pool[3]
    public GameObject GetUpper()
    {
		return GetSpecific(0);
	}
	public GameObject GetLeft()
	{
		return GetSpecific(1);
	}
	public GameObject GetLower()
	{
		return GetSpecific(2);
	}
	public GameObject GetRight()
	{
		return GetSpecific(3);
	}
	private GameObject GetSpecific(int index)
    {
		// grab item at the specific index
		for (int listIndex = 0; listIndex < poolArray[index].Count; listIndex++)
		{
			if (poolArray[index][listIndex].activeInHierarchy == false)
			{   // if the list at the listArrayIndex has a gameobject at one of its indexes but isn't active
				return poolArray[index][listIndex];
			}
		}
		// in not found, instantiate new one and return it
		GameObject pooledObject = Instantiate(poolArray[index][0]); 
		pooledObject.SetActive(false);
		poolArray[index].Add(pooledObject);
		return pooledObject;
	}
}
