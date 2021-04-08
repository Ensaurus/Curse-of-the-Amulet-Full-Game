using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpiritInteraction : MonoBehaviour
{

    public GameObject next;
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
        SceneManager.LoadScene("IntroScene");
    }
}
