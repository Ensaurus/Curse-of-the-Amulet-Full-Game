using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : Singleton<SceneManager>
{
    // width and height of current level map, map extends from (0,0,0) to levelDimensions
	public float levelHeight;
    public float levelWidth;	
	private Vector2 levelDimensions;

    private void Start() {
        levelDimensions.Set(levelWidth, levelHeight);
	}
}