using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class CameraHandlerSkript : MonoBehaviour
{
    // Speed values
    private float speed = 100;
    private float zoomSpeed = 10000;

    // Camera
    public Camera cam;

    void Update()
    {
        transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * speed,
            0, Input.GetAxis("Vertical") * Time.deltaTime * speed);
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView + (Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomSpeed), 20, 100);
    }
}