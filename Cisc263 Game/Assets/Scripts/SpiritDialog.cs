using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpiritDialog : MonoBehaviour
{
    public GameObject NPCBubble;
    public GameObject talkOption;
    public GameObject noOption;



    private void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player")){
            //Debug.Log("NPC Activated");
            Time.timeScale = 0f;
            NPCBubble.SetActive(true);
            talkOption.SetActive(true);
            noOption.SetActive(true);
        }
    }

    public void Talk(){
        //Debug.Log("NPC Chatting");
        Time.timeScale = 1f; 
        NPCBubble.SetActive(false);
        talkOption.SetActive(false);
        noOption.SetActive(false);
        SceneManager.LoadScene("NPC");
    }

    public void NoTalk(){
        //Debug.Log("No NPC Chatting");
        Time.timeScale = 1f;
        NPCBubble.SetActive(false);
        talkOption.SetActive(false);
        noOption.SetActive(false);
    }
}
