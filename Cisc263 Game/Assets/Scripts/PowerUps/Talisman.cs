using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talisman : MonoBehaviour, PowerUp
{
    public int GetAmount()
    {
        throw new System.NotImplementedException();
    }

    public string GetDescription()
    {
        return "Less charge used while turned to stone.";
    }

    public string GetItem()
    {
        throw new System.NotImplementedException();
    }

    public string GetName()
    {
        return "Talisman";
    }

    public bool isSingleUse()
    {
        return true;
    }

    public bool isUseable()
    {
        return false;
    }

    public void OnCollect()
    {
        Amulet.Instance.talismanBuff = 1.5f;
    }
}
