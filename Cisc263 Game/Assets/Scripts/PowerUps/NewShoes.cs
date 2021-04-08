using UnityEngine;

public class NewShoes : MonoBehaviour, PowerUp
{
    public int GetAmount()
    {
        throw new System.NotImplementedException();
    }

    public string GetDescription()
    {
        return "Child's 11. A perfect fit... speed increased.";
    }

    public string GetItem()
    {
        throw new System.NotImplementedException();
    }

    public string GetName()
    {
        return "New Shoes";
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
        PlayerController.Instance.speed = PlayerController.Instance.speed * 1.5f;
    }

}
