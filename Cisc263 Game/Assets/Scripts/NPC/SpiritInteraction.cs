using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SpiritInteraction : MonoBehaviour
{
    public GameObject options;
    public GameObject doneButton;
    public GameObject R1;
    public GameObject R2;
    public GameObject R3;
    public GameObject R4;
    public GameObject R5;
    public GameObject R6;
    public GameObject R7;
    public GameObject R8;
    public GameObject R9;
    public GameObject R10;
    
    public void nextQuestion(){
        R1.SetActive(false);
        R2.SetActive(false);
        R3.SetActive(false);
        R4.SetActive(false);
        R5.SetActive(false);
        R6.SetActive(false);
        R7.SetActive(false);
        R8.SetActive(false);
        R9.SetActive(false);
        R10.SetActive(false);
    }
    public void Question1(){
        nextQuestion();
        R1.SetActive(true);
    }
    public void Question2(){
        nextQuestion();
        R2.SetActive(true);
    }
    public void Question3(){
        nextQuestion();
        R3.SetActive(true);
    }
    public void Question4(){
        nextQuestion();
        R4.SetActive(true);
    }public void Question5(){
        nextQuestion();
        R5.SetActive(true);
    }
    public void Question6(){
        nextQuestion();
        R6.SetActive(true);
    }public void Question7(){
        nextQuestion();
        R7.SetActive(true);
    }
    public void Question8(){
        nextQuestion();
        R8.SetActive(true);
    }
    public void Question9(){
        nextQuestion();
        R9.SetActive(true);
    }
    public void Question10(){
        nextQuestion();
        R10.SetActive(true);
    }

//To Take you back to the intro scene
    public void Done(){
        nextQuestion();
        options.SetActive(false);
        doneButton.SetActive(false);
        // just grabbing one of the textboxes and replacing the text cuz I dont wanna deal with making new gameobjects for it
        TextMeshProUGUI textBox = R1.GetComponent<TextMeshProUGUI>();
        textBox.text = "By the way, I left you a chest by the portal with some items to help you along. Lost souls can point you in the right direction, cameras will help you" +
            " survey the land and sigils can hold the beasts at bay, but not for long.\n";
        textBox.fontSize = 12;
        R1.SetActive(true);
        StartCoroutine(FinalWords());
    }

    IEnumerator FinalWords()
    {
        TextMeshProUGUI textBox = R1.GetComponent<TextMeshProUGUI>();
        string finalWords = "Good luck kid.";
        float readingTimer = 10;
        while (readingTimer >= 0)
        {
            readingTimer -= Time.deltaTime;
            yield return null;
        }
        foreach (char i in finalWords)
        {
            textBox.text += i;
            yield return new WaitForSeconds(0.4f);
        }
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("IntroScene");
    }
}
