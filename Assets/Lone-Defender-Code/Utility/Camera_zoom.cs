using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_zoom : MonoBehaviour
{
    private Camera cam;
    //[SerializeField] private float zoom_speed = 10f;
    [SerializeField] protected float min_size = 2f;
    [SerializeField] protected float max_size = 100f;

    [SerializeField] protected float drag_speed = 100;
    [SerializeField] protected Vector3 drag_origin;


    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    protected void camera_zoom()
    {
        bool is_over_game_window = Input.mousePosition.x >= 0 &&
                        Input.mousePosition.x <= Screen.width &&
                        Input.mousePosition.y >= 0 &&
                        Input.mousePosition.y <= Screen.height;

        if(!is_over_game_window)
        {
            return;
        }

        float mouse_change = Input.mouseScrollDelta.y;

        if (mouse_change != 0)
        {
            Vector3 mouse_intial_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            float new_size = cam.orthographicSize - mouse_change; // alternate with zoom_speed:  cam.orthographicSize - mouse_change * zoom_speed

            cam.orthographicSize = Mathf.Clamp(new_size, min_size, max_size);

            Vector3 mouse_after = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector3 diff = mouse_intial_pos - mouse_after;
            transform.position += diff;
        }
    }

    protected void camera_pan()
    {
        if(Input.GetMouseButtonDown(2))
        {
            drag_origin = Input.mousePosition;
            return;
        }

        if(Input.GetMouseButton(2))
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - drag_origin);
            Vector3 move = new Vector3(pos.x * drag_speed, pos.y * drag_speed, 0);

            transform.Translate(-move, Space.Self);

            drag_origin = Input.mousePosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        camera_zoom();
        camera_pan();
    }
}
