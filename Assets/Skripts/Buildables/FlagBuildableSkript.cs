using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagBuildableSkript : MonoBehaviour
{
    private bool mouseOver;
    private GameObject gui;
    private bool guiActive;

    void Start()
    {
        mouseOver = false;
        transform.position = new Vector3(transform.position.x,
            GameHandler.ActiveTerrain.SampleHeight(transform.position) + GameHandler.FlagBuildableYOffset,
            transform.position.z);
        guiActive = false;
    }

    private void Update()
    {
        if (guiActive)
        {
            if (gui != null)
            {
                gui.transform.position = Camera.main.WorldToScreenPoint(transform.position) + new Vector3(
                    150, 80, 0);
            }
            else
            {
                guiActive = false;
            }
        }
    }

    public void ReplaceWithFlag()
    {
    }

    private void OnMouseDown()
    {
        if(!GameHandler.GUIActive)
        {
            gui = Instantiate(GameHandler.FahnenerzeugungGUIPrefab, Camera.main.WorldToScreenPoint(transform.position) +
                                                                    new Vector3(
                                                                        150, 80, 0), Quaternion.identity,
                GameHandler.MainCanvas.transform);
            guiActive = true;
            GameHandler.GUIActive = true;
        }
    }

    private void OnMouseEnter()
    {
        if (!mouseOver)
        {
            transform.localScale += new Vector3(1, 1, 1);
            mouseOver = true;
        }
    }

    private void OnMouseExit()
    {
        if (mouseOver)
        {
            transform.localScale -= new Vector3(1, 1, 1);
            mouseOver = false;
        }
    }
}