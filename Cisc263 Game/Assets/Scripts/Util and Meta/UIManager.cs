using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public TextMeshProUGUI amuletChargeText;
    public TextMeshProUGUI lanternChargeText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI textAbovePlayer;
    public TextMeshProUGUI levelTransitionText;
    public TextMeshProUGUI newPowerUpDisplay;
    public TextMeshProUGUI activeItem;
    public TextMeshProUGUI qPrompt;
    public TextMeshProUGUI ePrompt;
    public Image blackBackground;
    public Image jumpScare;
    public bool isScaring = false;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.JumpScare.AddListener(DisplayJumpScare);
        EventManager.Instance.EnemyStateChange.AddListener(DisplayTracking);
        EventManager.Instance.PowerUpCollected.AddListener(NewPowerUp);
        EventManager.Instance.ItemSwap.AddListener(ItemSwap);
        EventManager.Instance.ItemUsed.AddListener(ItemUsed);
        updateAmuletCharge();
        updateLanternCharge();
    }

    // Update is called once per frame
    void Update()
    {
        if (Amulet.Instance.isActive || Amulet.Instance.isCharging)
        {
            if (Amulet.Instance.isActive)
            {
                amuletChargeText.color = new Color(255, 0, 0);
            }
            else
            {
                amuletChargeText.color = new Color(0, 255, 0);
            }
            updateAmuletCharge();
        }
        else
        {
            amuletChargeText.color = new Color(255, 255, 255);
        }

        if (Lantern.Instance.isOn || Lantern.Instance.triggered)
        {
            if (Lantern.Instance.isOn)
            {
                lanternChargeText.color = new Color(255, 0, 0);
            }
            else
            {
                lanternChargeText.color = new Color(0, 255, 0);
            }
            updateLanternCharge();
        }
        else
        {
            lanternChargeText.color = new Color(255, 255, 255);
        }
    }

    private void ItemSwap(GameObject newActive)
    {
        StartCoroutine(HandleItemSwap(newActive));
    }

    IEnumerator HandleItemSwap(GameObject newActive)
    {
        qPrompt.color = new Color(0, 255, 0);
        PowerUp script = newActive.GetComponent<PowerUp>();
        name = script.GetName();
        activeItem.text = newActive.GetComponent<PowerUp>().GetName();
        float timer = 0.5f;
        while (timer >= 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        qPrompt.color = new Color(255, 255, 255);
    }

    private void ItemUsed(GameObject newActive)
    {
        StartCoroutine(HandleItemSwap(newActive));
    }

    IEnumerator HandleItemUsed(GameObject newActive)
    {
        qPrompt.color = new Color(0, 255, 0);
        PowerUp script = newActive.GetComponent<PowerUp>();
        name = script.GetName();
        activeItem.text = newActive.GetComponent<PowerUp>().GetName();
        float timer = 0.5f;
        while (timer >= 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        qPrompt.color = new Color(255, 255, 255);
    }

    private void NewPowerUp(GameObject newPowerUp)
    {
        PowerUp script = newPowerUp.GetComponent<PowerUp>();
        StartCoroutine(DisplayNewPowerUp(script));
    }

    IEnumerator DisplayNewPowerUp(PowerUp newPowerUp)
    {
        newPowerUpDisplay.text = "" + newPowerUp.GetName() + " collected.\n" + newPowerUp.GetDescription();
        newPowerUpDisplay.gameObject.SetActive(true);
        float timer = 3;
        while (timer >= 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        newPowerUpDisplay.gameObject.SetActive(false);
    }

    private void updateAmuletCharge()
    {
        amuletChargeText.text = "Amulet Charge: " + Mathf.Round(Amulet.Instance.charge);
    }

    private void updateLanternCharge()
    {
        lanternChargeText.text = "Lantern Charge: " + Mathf.Round(Lantern.Instance.currentEnergy);
    }


    private void DisplayJumpScare()
    {
        // Debug.Log("made it here");
        StartCoroutine(JumpScare());
    }

    private void DisplayTracking(EnemyAI.State newState)
    {
        // Debug.Log("Display Tracking called");
        if (newState == EnemyAI.State.TRACKING)
        {
            textAbovePlayer.gameObject.SetActive(true);
        }
        else
        {
            // Debug.Log("removing tracking text");
            textAbovePlayer.gameObject.SetActive(false);
        }
    }

    IEnumerator JumpScare()
    {
        isScaring = true;
        // Debug.Log("JumpScare");
        float timer = 0;
        float width = 1;
        float height = 1;
        jumpScare.gameObject.SetActive(true);

        while (timer <= 0.5f)
        {
            jumpScare.transform.localScale = new Vector2(width, height);
            width += 0.1f;
            height += 0.1f;
            timer += Time.deltaTime;
            yield return null;
        }

        jumpScare.gameObject.SetActive(false);
        isScaring = false;
    }

    



    public void DisplayGameOver()
    {
        gameOverText.gameObject.SetActive(true);
    }



    public void LevelTransitionText()
    {
        StartCoroutine(FadeInAndOut(blackBackground));
        levelTransitionText.text = "Level Complete!";
        levelTransitionText.gameObject.SetActive(true);
    }

    IEnumerator FadeInAndOut(Image obj)
    {
        // make image transparent
        Color objColor = obj.color;
        objColor.a = 0;
        obj.color = objColor;
        obj.gameObject.SetActive(true);

        int fadeSpeed = 5;  // change this to change fade speed
        while (obj.color.a < 1){
            objColor.a += Time.deltaTime * fadeSpeed;
            obj.color = objColor;
            yield return null;
        }
        StartCoroutine(WaitBeforeFadeOut(obj));
    }

    IEnumerator WaitBeforeFadeOut(Image obj)
    {
        float timer = 3;    // wait 3 secs
        while (timer >= 1.5)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        // change text halfway through transition
        levelTransitionText.text = "Level " + SceneController.Instance.currentLevel;
        while (timer >= 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        StartCoroutine(FadeOut(obj));
    }


    IEnumerator FadeOut(Image obj)
    {
        Color objColor = obj.color;

        int fadeSpeed = 5;  // change this to change fade speed
        while (obj.color.a > 0.1f)
        {
            objColor.a -= Time.deltaTime * fadeSpeed;
            obj.color = objColor;
            yield return null;
        }
        obj.gameObject.SetActive(false);
        levelTransitionText.gameObject.SetActive(false);
        EventManager.Instance.FadeComplete.Invoke();
    }
}
