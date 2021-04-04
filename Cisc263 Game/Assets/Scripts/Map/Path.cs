using UnityEngine;

public class Path : MonoBehaviour
{
    public GameObject powerUpPosObject;
    public GameObject exitPosObject;
    public Vector2 powerUpPos;
    public Vector2 exitPos;
    private void OnEnable()
    {
        powerUpPos = powerUpPosObject.transform.position;
        exitPos = exitPosObject.transform.position;
    }
}
