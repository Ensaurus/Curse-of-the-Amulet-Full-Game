using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraX3 : MonoBehaviour, PowerUp
{
    public string GetName()
    {
        return "Camera x3";
    }
    public string GetDescription()
    {
        return "3 brand new cameras.";
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
        return "camera";
    }

    public int GetAmount()
    {
        return 3;
    }
}
