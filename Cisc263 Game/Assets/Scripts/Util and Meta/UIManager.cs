﻿using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public TextMeshProUGUI amuletChargeText;
    public TextMeshProUGUI lanternChargeText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI textAbovePlayer;
    public Image jumpScare;
    public bool isScaring = false;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.JumpScare.AddListener(DisplayJumpScare);
        EventManager.Instance.EnemyStateChange.AddListener(DisplayTracking);
        updateAmuletCharge();
        updateLanternCharge();
    }

    // Update is called once per frame
    void Update()
    {
        if (Amulet.Instance.isActive || Amulet.Instance.isCharging)
        {
            updateAmuletCharge();
        }

        if (Lantern.Instance.isOn)
        {
            updateLanternCharge();
        }
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
        textAbovePlayer.text = "Level Complete!";
        textAbovePlayer.gameObject.SetActive(true);
    }
}
