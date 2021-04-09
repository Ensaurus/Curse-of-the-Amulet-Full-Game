using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>, Item
{
    /* key for this item in inventory: "camera"
     * 
     */
    [SerializeField] private GameObject Camera;
  
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject cameraDrop;
    [SerializeField] private GameObject nightModeLight;
    
    private Transform dropTransform;
    private Vector3 dropPosition;
    private List<Vector3> cameraPositions = new List<Vector3>();

    [SerializeField] private int numCameras;
    [SerializeField] private int CurrentCamera;
    private List<GameObject> SpawnedCamerasObjectList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
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

        //cycle through cameras
        if (Input.GetKeyDown(KeyCode.Tab) && numCameras > 1) {
            StartCoroutine(switchCamera());
        }
    }
    public void Use()
    {
        StartCoroutine(dropCamera());
    }
    public string GetName()
    {
        return "Camera"; 
    }

    public int GetAmount()
    {
        return Inventory.Instance.items["camera"];
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
        switchColorFromRed(CurrentCamera);
        //Find the position of the next camera in the list
        if(CurrentCamera + 1 < numCameras){
            CurrentCamera += 1;
            Camera realCamera = Camera.GetComponent<Camera>();
            realCamera.orthographicSize = 10;
            nightModeLight.SetActive(true);
        }
        else{
            Camera realCamera = Camera.GetComponent<Camera>();
            realCamera.orthographicSize = 7.5f;
            CurrentCamera = 0;
            nightModeLight.SetActive(false);
        }
        switchColorToRed(CurrentCamera);

        //Change the camera's location
        Vector3 temp = new Vector3(cameraPositions[CurrentCamera].x, cameraPositions[CurrentCamera].y, Camera.transform.position.z);
        Camera.transform.position = temp;
        
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
        //For the minimap
        void switchColorFromRed(int CurCamNum)
        {
        GameObject white;
        GameObject red;
        if (CurCamNum == 0){
            white = player.transform.GetChild(0).gameObject;
            red = player.transform.GetChild(1).gameObject;
        }
        else {
            GameObject cameraOb = SpawnedCamerasObjectList[CurCamNum - 1];
            white = cameraOb.transform.GetChild(0).gameObject;
            red = cameraOb.transform.GetChild(1).gameObject;
        }
        white.SetActive(true);
        red.SetActive(false);
    }

    void switchColorToRed(int CurCamNum)
        {
        GameObject white;
        GameObject red;
        if (CurCamNum == 0){
            white = player.transform.GetChild(0).gameObject;
            red = player.transform.GetChild(1).gameObject;
        }
        else {
            GameObject cameraOb = SpawnedCamerasObjectList[CurCamNum - 1];
            white = cameraOb.transform.GetChild(0).gameObject;
            red = cameraOb.transform.GetChild(1).gameObject;
        }
        white.SetActive(false);
        red.SetActive(true);

    }
}
