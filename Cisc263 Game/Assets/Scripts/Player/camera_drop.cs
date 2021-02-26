using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_drop : MonoBehaviour
{
    private bool camera_is_dropped;
    private int camera_counter;
    private Vector3 drop_position;

    [SerializeField] private GameObject player;

    [SerializeField] private GameObject drop;

    [SerializeField] private GameObject cam_1;
    [SerializeField] private GameObject cam_2;
    // Start is called before the first frame update
    void Start()
    {
        camera_is_dropped = false;
        camera_counter = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C)){
            if(!camera_is_dropped){
                camera_is_dropped = true;
                drop.SetActive(true);
                cam_2.SetActive(false);
                cam_1.SetActive(true);

                drop_position = player.transform.position;
                drop.transform.position = drop_position;

            }
            else{
                switch_camera();
            }
        }
    }

    void switch_camera(){
        if(camera_counter % 2 == 0){
            cam_1.SetActive(false);
            cam_2.SetActive(true);
        }
        else{
            cam_1.SetActive(true);
            cam_2.SetActive(false);
        }
        camera_counter++;
    }
}
