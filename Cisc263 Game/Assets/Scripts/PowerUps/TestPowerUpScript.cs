using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TestPowerUpScript : MonoBehaviour, PowerUp
{
    public string GetName()
    {
        return "Test Powerup";
    }
    public string GetDescription()
    {
        return "This powerup is a test and doesn't do much.";
    }
    public void Activate()
    {
        Debug.Log("Item activated!");
    }
    public bool isSingleUse()
    {
        return false;
    }
    public bool isUseable()
    {
        return true;
    }

    public void OnCollect()
    {
        throw new System.NotImplementedException();
    }

    public string GetItem()
    {
        return "test";
    }

    public int GetAmount()
    {
        return 5;
    }
}
