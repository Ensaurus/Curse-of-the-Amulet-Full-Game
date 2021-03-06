using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    public TextMeshProUGUI amuletChargeText;
    public TextMeshProUGUI lanternChargeText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI textAbovePlayer;
    public TextMeshProUGUI levelTransitionText;
    public TextMeshProUGUI newPowerUpName;
    public TextMeshProUGUI newPowerUpDescription;
    public TextMeshProUGUI activeItem;
    public TextMeshProUGUI qPrompt;
    public TextMeshProUGUI ePrompt;
    public GameObject gameOverScreen;
    public GameObject enterPortalScreen;
    public TextMeshProUGUI enterPortalText;
    public Image blackBackground;
    public Image jumpScare;
    public bool isScaring = false;

    public TextMeshProUGUI openChestText;
    [SerializeField] private AudioSource DeathSound;

    // Start is called before the first frame update
    void Start()
    {
        //chest text
        //openChestText.text = "Press E to open chest...";
        EventManager.Instance.AttemptedExitWithEnoughCharge.AddListener(AskToEnterPortal);
        EventManager.Instance.PortalNotTaken.AddListener(PutAwayPortalScreen);
        EventManager.Instance.JumpScare.AddListener(DisplayJumpScare);
        EventManager.Instance.PowerUpCollected.AddListener(NewPowerUpHandler);
        EventManager.Instance.ItemSwap.AddListener(ItemSwap);
        EventManager.Instance.ItemUsed.AddListener(ItemUsed);
        EventManager.Instance.ItemIncrease.AddListener(ItemIncrease);
        EventManager.Instance.FailedPortalEntry.AddListener(FailedPortalEntryHandler);
        updateAmuletCharge();
        updateLanternCharge();
        activeItem.text = "Camera x" + CameraManager.Instance.GetAmount();
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

    private void FailedPortalEntryHandler(int chargeRequired)
    {
        StartCoroutine(DisplayChargeRequired(chargeRequired));
    }

    IEnumerator DisplayChargeRequired(int chargeRequired)
    {
        textAbovePlayer.text = "You require " + chargeRequired + " amulet charge to enter this portal.";
        textAbovePlayer.gameObject.SetActive(true);
        float timer = 6f;
        while (timer >= 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        textAbovePlayer.gameObject.SetActive(false);
    }

    #region Inventory
    private void ItemSwap(Item newActive)
    {
        StartCoroutine(HandleItemSwap(newActive));
    }

    IEnumerator HandleItemSwap(Item newActive)
    {
        string name;
        int amount;
        if (newActive == null)
        {
            name = "Empty";
            activeItem.text = name;
        }
        else
        {
            name = newActive.GetName();
            amount = newActive.GetAmount();
            activeItem.text = name + " x" + amount;
        }
        qPrompt.color = new Color(0, 255, 0);
        float timer = 0.5f;
        while (timer >= 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        qPrompt.color = new Color(255, 255, 255);
    }

    private void ItemUsed(Item active)
    {
        StartCoroutine(HandleItemUsed(active));
    }

    IEnumerator HandleItemUsed(Item active)
    {
        string name;
        int amount;
        if (active == null)
        {
            name = "Empty";
            activeItem.text = name;
        }
        else
        {
            name = active.GetName();
            amount = active.GetAmount();
            activeItem.text = name + " x" + amount;
        }
        ePrompt.color = new Color(0, 255, 0);
        float timer = 0.5f;
        while (timer >= 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        ePrompt.color = new Color(255, 255, 255);
    }

    private void ItemIncrease(Item newItem)
    {
        StartCoroutine(UpdateActiveItem(newItem));
    }
    IEnumerator UpdateActiveItem(Item newItem)
    {
        string name = newItem.GetName();
        int amount = newItem.GetAmount();
        activeItem.text = name + " x" + amount;
        activeItem.color = new Color(0, 255, 0);
        float timer = 1;
        while (timer >= 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        activeItem.color = new Color(255, 255, 255);
    }


    private void NewPowerUpHandler(GameObject newPowerUp)
    {
        PowerUp script = newPowerUp.GetComponent<PowerUp>();
        StartCoroutine(DisplayNewPowerUp(script));
    }

    IEnumerator DisplayNewPowerUp(PowerUp newPowerUp)
    {
        newPowerUpName.text = newPowerUp.GetName() + " collected.";
        newPowerUpDescription.text = newPowerUp.GetDescription();
        newPowerUpName.gameObject.SetActive(true);
        newPowerUpDescription.gameObject.SetActive(true);
        float timer = 6;
        while (timer >= 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        newPowerUpName.gameObject.SetActive(false);
        newPowerUpDescription.gameObject.SetActive(false);
        newPowerUpName.text = "";   // set text to empty so this can be made active/deactivated in level transition to account for overlappin text
        newPowerUpDescription.text = "";
    }

    #endregion

    #region amulet/lantern
    public void updateAmuletCharge()
    {
        //Max charge display
        if(Amulet.Instance.charge >= Amulet.Instance.maxCharge){
            amuletChargeText.text = "Amulet Charge: " + Mathf.Round(Amulet.Instance.charge) + " (Max)";
        }
        else{
            amuletChargeText.text = "Amulet Charge: " + Mathf.Round(Amulet.Instance.charge);
        }
        
    }

    private void updateLanternCharge()
    {
        //Max charge display
        if(Lantern.Instance.currentEnergy >= Lantern.Instance.maxEnergy){
            lanternChargeText.text = "Lantern Charge: " + Mathf.Round(Lantern.Instance.currentEnergy) + " (Max)";
        }
        else{
            lanternChargeText.text = "Lantern Charge: " + Mathf.Round(Lantern.Instance.currentEnergy);
        }
        
    }

    #endregion


    #region game over
    private void DisplayJumpScare()
    {
        // Debug.Log("made it here");
        StartCoroutine(JumpScare());
    }

    IEnumerator JumpScare()
    {
        DeathSound.Play();
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
        DisplayGameOver();
    }


    public void DisplayGameOver()
    {
        gameOverText.gameObject.SetActive(true);
        StartCoroutine(FadeInGameOverScreen());
    }

    IEnumerator FadeInGameOverScreen()
    {
        // make image transparent
        Color backgroundColor = blackBackground.color;
        backgroundColor.a = 0;
        blackBackground.color = backgroundColor;
        blackBackground.gameObject.SetActive(true);

        int fadeSpeed = 5;  // change this to change fade speed
        while (blackBackground.color.a < 1)
        {
            backgroundColor.a += Time.deltaTime * fadeSpeed;
            blackBackground.color = backgroundColor;
            yield return null;
        }
        gameOverScreen.SetActive(true);
        Time.timeScale = 0;
    }

    private void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }

    private void ReturnToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    #endregion




    #region lvl transition

    private void AskToEnterPortal()
    {
        newPowerUpName.gameObject.SetActive(false);
        newPowerUpDescription.gameObject.SetActive(false);
        enterPortalText.text = "Would you like to spend " + Exit.Instance.requiredEnergy + " amulet charge to enter the portal?";
        
        enterPortalScreen.SetActive(true);
    }

    private void PutAwayPortalScreen()
    {
        newPowerUpName.gameObject.SetActive(true);
        newPowerUpDescription.gameObject.SetActive(true);
        enterPortalScreen.SetActive(false);
    }

    public void LevelTransitionText()
    {
        enterPortalScreen.SetActive(false);
        StartCoroutine(FadeInAndOut(blackBackground));
        levelTransitionText.text = "Level Complete!";
        levelTransitionText.gameObject.SetActive(true);

        //camera destroy
        CameraManager.Instance.Reset();
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
        newPowerUpName.gameObject.SetActive(true);  // start displaying interupted text again
        newPowerUpDescription.gameObject.SetActive(true);
        EventManager.Instance.FadeComplete.Invoke();
    }

    #endregion

    //For the powerup chests
    public void openChestTextDisplay()
    {
        openChestText.gameObject.SetActive(true);
    }

    public void openChestTextHide()
    {
        openChestText.gameObject.SetActive(false);
    }
}
