using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private GameObject Camera;
    [SerializeField] private int maxCameras;
    [SerializeField] private int curCameras;
    [SerializeField] private int initCameras;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject cameraDrop;
    private Transform dropTransform;
    private Vector3 dropPosition;
    private List<Vector3> cameraPositions = new List<Vector3>();
    
    [SerializeField] private int numCameras;
    [SerializeField] private int CurrentCamera;
    private List<GameObject> SpawnedCamerasObjectList = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        curCameras = initCameras;

        numCameras = 1;
        CurrentCamera = 0;

        //Add the player's position to the list
        cameraPositions.Add(player.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        //Keep updating the player's position
        cameraPositions[0] = player.transform.position;

        //drop camera
        if(Input.GetKeyDown(KeyCode.C) && curCameras > 0 && CurrentCamera == 0){
            
            StartCoroutine(dropCamera());
            curCameras -= 1;
        }

        //cycle through cameras
        if(Input.GetKeyDown(KeyCode.Tab) && numCameras > 1){
            StartCoroutine(switchCamera());
        }
    }

    IEnumerator dropCamera(){

        dropTransform = player.transform;
        
        dropPosition = dropTransform.position;

        //Save the position in a list
        cameraPositions.Add(dropPosition);
        //increment the number of cameras
        numCameras += 1;

        dropPosition.z = 0;
        
        SpawnedCamerasObjectList.Add(Instantiate(cameraDrop, dropPosition, dropTransform.rotation));
        

        yield return null;
    }

    IEnumerator switchCamera(){

        //Find the position of the next camera in the list
        if(CurrentCamera + 1 < numCameras){
            CurrentCamera += 1;
        }
        else{
            CurrentCamera = 0;
        }

        //Change the camera's location
        Vector3 temp = new Vector3(cameraPositions[CurrentCamera].x, cameraPositions[CurrentCamera].y, Camera.transform.position.z);
        Camera.transform.position = temp;
        
        print("test3");
        yield return null;
    }

    //Reset everything when starting new levels
    public void Reset()
    {
        //empty list and reset to just 1 camera
        cameraPositions.Clear();
        cameraPositions.Add(player.transform.position);

        //destroy objects
        for(int i = 0; i < numCameras - 1; i ++){
            Destroy(SpawnedCamerasObjectList[i]);
        }
        SpawnedCamerasObjectList.Clear();

        //reset other values
        CurrentCamera = 0;
        numCameras = 1;
    }

    //player can move only when not using cameras
    public bool checkMovingAllowed()
    {
        if(CurrentCamera == 0)
            return true;
        else    
            return false;
    }
}
