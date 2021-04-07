using UnityEngine;
public class CameraPickup : MonoBehaviour, PowerUp
{
    private int rng;
    [SerializeField] int minAmount;
    [SerializeField] int maxAmount;

    public string GetName()
    {
        return "Cameras x" + rng;
    }
    public string GetDescription()
    {
        string accountForPlural;
        switch (rng)
        {
            case 1:
                accountForPlural = "A brand new camera.";
                break;
            default:
                accountForPlural = "Brand new cameras.";
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
        return "camera";
    }

    public int GetAmount()
    {
        return rng;
    }
}
