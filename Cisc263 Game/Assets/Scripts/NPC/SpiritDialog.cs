using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpiritDialog : MonoBehaviour
{
    public GameObject NPCBubble;
    public GameObject talkOption;
    public GameObject noOption;
    public GameObject Player;

    IntroPlayerController playerScript;


    void Start(){
        playerScript = Player.GetComponent<IntroPlayerController>();
    }
    private void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player")){
            //stop player animations
            playerScript.isTalkingToSpirit = true;

            //Debug.Log("NPC Activated");
            Time.timeScale = 0f;
            NPCBubble.SetActive(true);
            talkOption.SetActive(true);
            noOption.SetActive(true);

            
        }
    }

    public void Talk(){
        //resume player animations
        playerScript.isTalkingToSpirit = false;

        //Debug.Log("NPC Chatting");
        Time.timeScale = 1f; 
        NPCBubble.SetActive(false);
        talkOption.SetActive(false);
        noOption.SetActive(false);
        ContinuityManager.Instance.npcTalkedTo = true;  // set flag in continuity manager
        ContinuityManager.Instance.portalEntered = false;
        SceneManager.LoadScene("NPC");
    }

    public void NoTalk(){
        //resume player animations
        playerScript.isTalkingToSpirit = false;
        
        //Debug.Log("No NPC Chatting");
        Time.timeScale = 1f;
        NPCBubble.SetActive(false);
        talkOption.SetActive(false);
        noOption.SetActive(false);
    }
}
