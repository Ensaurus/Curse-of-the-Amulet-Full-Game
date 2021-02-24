using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_movement : MonoBehaviour
{
    [SerializeField] private float mouse_sensitivity;
    [SerializeField] private float smoothing;
    private GameObject player;
    private Vector2 smooth_vel;
    private Vector2 current_pos;
    
    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent.gameObject;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        rotate_camera();
    }

    private void rotate_camera() {

        //Get X and Y values for mouse movement
        Vector2 mouse_input = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        //adjust sensitivity
        mouse_input = Vector2.Scale(mouse_input, new Vector2(mouse_sensitivity * smoothing, mouse_sensitivity * smoothing));

        //make camera movement smoother
        smooth_vel.x = Mathf.Lerp(smooth_vel.x, mouse_input.x, 1f / smoothing);
        smooth_vel.y = Mathf.Lerp(smooth_vel.y, mouse_input.y, 1f / smoothing);

        current_pos += smooth_vel;

        //actually move
        transform.localRotation = Quaternion.AngleAxis(-current_pos.y, Vector3.right);

        player.transform.localRotation = Quaternion.AngleAxis(current_pos.x, player.transform.up);


    }
}
