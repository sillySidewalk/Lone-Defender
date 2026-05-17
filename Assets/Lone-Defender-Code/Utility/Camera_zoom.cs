using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_zoom : MonoBehaviour
{
    private Camera cam;
    //[SerializeField] private float zoom_speed = 10f;
    [SerializeField] private float min_size = 2f;
    [SerializeField] private float max_size = 100f;


    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        float mouse_change = Input.mouseScrollDelta.y;

        if(mouse_change != 0 )
        {
            float new_size = cam.orthographicSize - mouse_change; // alternate with zoom_speed:  cam.orthographicSize - mouse_change * zoom_speed

            cam.orthographicSize = Mathf.Clamp(new_size, min_size, max_size);
        }
    }
}
