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
        return "Beasts now have a harder time finding your scent.";
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
        Debug.Log(ScentSpawner.Instance.stinkiness);
        ScentSpawner.Instance.stinkiness = ScentSpawner.Instance.stinkiness / 2;
    }
}
