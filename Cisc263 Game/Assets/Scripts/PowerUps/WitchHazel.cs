using UnityEngine;
public class WitchHazel : MonoBehaviour, PowerUp
    // Scent nodes last half as long
{
    public int GetAmount()
    {
        throw new System.NotImplementedException();
    }

    public string GetDescription()
    {
        return "You rub the foul odor on your clothes to hide your scent.";
    }

    public string GetItem()
    {
        throw new System.NotImplementedException();
    }

    public string GetName()
    {
        return "Witch's Hazel";
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
        ScentSpawner.Instance.stinkiness = ScentSpawner.Instance.stinkiness / 2;
    }
}
