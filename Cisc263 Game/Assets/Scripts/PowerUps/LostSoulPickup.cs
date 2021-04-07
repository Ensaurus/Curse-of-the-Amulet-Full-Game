using UnityEngine;
public class LostSoulPickup : MonoBehaviour, PowerUp
{
    private int rng;
    [SerializeField] int minAmount;
    [SerializeField] int maxAmount;

    public string GetName()
    {
        return "Lost Soul x" + rng;
    }
    public string GetDescription()
    {
        string accountForPlural;
        switch (rng)
        {
            case 1:
                accountForPlural = "A lost soul looking for a way out, same as you.";
                break;
            default:
                accountForPlural = "Lost souls looking for a way out, same as you.";
                break;
        }
        return accountForPlural;
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
        rng = Random.Range(minAmount, maxAmount + 1);
    }

    public string GetItem()
    {
        return "soul";
    }

    public int GetAmount()
    {
        return rng;
    }
}