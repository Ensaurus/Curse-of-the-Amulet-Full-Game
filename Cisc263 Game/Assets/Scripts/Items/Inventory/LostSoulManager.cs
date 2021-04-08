using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostSoulManager : Singleton<LostSoulManager>, Item
{
    /*Key for inventory: "soul"
     * 
     * 
     */
    [SerializeField] private GameObject lostSoul;
    [SerializeField] private GameObject player;

    public int GetAmount()
    {
        return Inventory.Instance.items["soul"];
    }

    public string GetName()
    {
        return "Lost Soul";
    }

    public void Use()
    {
        Vector3 playerPos = player.transform.position;
        playerPos.z = -0.5f; // light needs to be off ground a bit to be seen
        Instantiate(lostSoul, playerPos, player.transform.rotation);
    }
}
