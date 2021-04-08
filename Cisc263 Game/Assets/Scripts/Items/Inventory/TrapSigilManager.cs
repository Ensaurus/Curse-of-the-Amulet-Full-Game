using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSigilManager : Singleton<TrapSigilManager>, Item
{
    /* inventory key for this item is: "trap"
     * 
     * 
     * 
     */
    public List<GameObject> activeTraps = new List<GameObject>();
    [SerializeField] private GameObject sigilTrap;
    [SerializeField] private GameObject player;
    
    private void Start()
    {
        EventManager.Instance.LevelCompleted.AddListener(Reset);
    }


    private void Reset()
    {
        for (int i=0; i<activeTraps.Count; i++)
        {
            Destroy(activeTraps[i]);
        }
        activeTraps = new List<GameObject>();
    }

    public void Use()
    {
        Vector2 spawnPt = player.transform.position; 
        activeTraps.Add(Instantiate(sigilTrap, spawnPt, player.transform.rotation));
    }

    public string GetName()
    {
        return "Trap Sigil";
    }

    public int GetAmount()
    {
        return Inventory.Instance.items["trap"];
    }
}
