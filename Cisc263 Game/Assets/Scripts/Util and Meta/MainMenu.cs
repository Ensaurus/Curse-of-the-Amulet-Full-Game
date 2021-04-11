using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject ContinuityManager;
    public void PlayGame(){
        if (GameObject.Find("ContinuityManager(Clone)") == null)
        {
            Instantiate(ContinuityManager); // if first time starting game, add continuity manager
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
