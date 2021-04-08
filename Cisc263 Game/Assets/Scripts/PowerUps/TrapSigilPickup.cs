using UnityEngine;

public class TrapSigilPickup : MonoBehaviour, PowerUp
{
    public int minAmount;
    public int maxAmount;
    private int rng;
    public int GetAmount()
    {
        return rng;
    }

    public string GetDescription()
    {
        string accountForPlural;
        switch (rng)
        {
            case 1:
                accountForPlural = "A powerful seal sure to stop any beast in its tracks.";
                break;
            default:
                accountForPlural = "Powerful seals, sure to stop any beast in its tracks.";
                break;
        }
        return accountForPlural;
    }

    public string GetItem()
    {
        return "trap";
    }

    public string GetName()
    {
        return "Trap Sigil x" + rng;
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
}
