using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundControl : MonoBehaviour
{

    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private AudioSource lvlTransitionMusic;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.LevelCompleted.AddListener(TransitionStart);
        EventManager.Instance.FadeComplete.AddListener(TransitionEnd);
    }

    private void TransitionStart()
    {
        backgroundMusic.Stop();
        lvlTransitionMusic.Play();
    }

    private void TransitionEnd()
    {
        backgroundMusic.Play();
    }
}