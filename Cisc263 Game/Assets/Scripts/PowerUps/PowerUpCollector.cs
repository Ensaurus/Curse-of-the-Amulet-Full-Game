using UnityEngine;

public class PowerUpCollector : MonoBehaviour
{
    // handles non-useable powerups
    private void Start()
    {
        EventManager.Instance.PowerUpCollected.AddListener(PowerUpHandler);
    }

    private void PowerUpHandler(GameObject newPowerUp)
    {
        PowerUp script = newPowerUp.GetComponent<PowerUp>();
        if (script.isSingleUse())
        {
            script.OnCollect();
        }
    }
}
