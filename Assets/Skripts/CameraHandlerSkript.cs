using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

/// <summary>
/// Script for the CameraHandler GameObject
/// </summary>
public class CameraHandlerSkript : MonoBehaviour
{
    // Speed values
    /// <summary>
    /// The movement speed of the camera
    /// </summary>
    private float speed = 100;
    /// <summary>
    /// The zoom speed of the camera
    /// </summary>
    private float zoomSpeed = 10000;
    
    // Y-Ofset
    /// <summary>
    /// The default distance from the camera to the ground TODO remove maybe
    /// </summary>
    private float yOffset = 20;

    // Camera
    public Camera cam;

    void Update()
    {
        // The input of the w a s d or arrow keys gets translated to camera movement
        transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * speed,
            0, Input.GetAxis("Vertical") * Time.deltaTime * speed);
        transform.position = new Vector3(transform.position.x, GameHandler.ActiveTerrain.SampleHeight(transform.position) + yOffset,
            transform.position.z);
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView + (Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomSpeed), 20, 100);
    }
}