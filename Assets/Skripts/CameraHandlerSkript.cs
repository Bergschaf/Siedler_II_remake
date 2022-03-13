using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class CameraHandlerSkript : MonoBehaviour
{
    // Speed values
    private float speed = 100;
    private float zoomSpeed = 10000;
    
    // Y-Ofset
    private float yOffset = 20;

    // Camera
    public Camera cam;

    void Update()
    {
        transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * speed,
            0, Input.GetAxis("Vertical") * Time.deltaTime * speed);
        transform.position = new Vector3(transform.position.x, GameHandler.ActiveTerrain.SampleHeight(transform.position) + yOffset,
            transform.position.z);
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView + (Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomSpeed), 20, 100);
    }
}