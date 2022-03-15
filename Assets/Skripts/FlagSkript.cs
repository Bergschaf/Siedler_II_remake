using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagSkript : MonoBehaviour
{
    private bool guiActive;

    private GameObject gui;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x,
            GameHandler.ActiveTerrain.SampleHeight(transform.position),
            transform.position.z);
    }
    
    
    private void Update()
    {
        if (guiActive)
        {
            if (gui == null)
            {
                guiActive = false;
                GameHandler.GUIActive = false;
            }
            else
            {
                gui.transform.position = Camera.main.WorldToScreenPoint(transform.position) + new Vector3(150, 80, 0);
            }
        }
    }

    private void OnMouseDown()
    {
        if (!GameHandler.GUIActive)
        {
            gui = Instantiate(GameHandler.FahnenmenuPrefab,
                Camera.main.WorldToScreenPoint(transform.position) + new Vector3(150, 80, 0), Quaternion.identity,
                GameHandler.MainCanvas.transform);
            guiActive = true;
            GameHandler.GUIActive = true;
        }
    }

}