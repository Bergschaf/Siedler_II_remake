using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class FlagBuildableSkript : MonoBehaviour
{
    private bool mouseOver;
    private GameObject gui;
    private bool guiActive;
    private bool roadBuidlingGUIActive;

    void Start()
    {
        mouseOver = false;
        transform.position = new Vector3(transform.position.x,
            GameHandler.ActiveTerrain.SampleHeight(transform.position) + GameHandler.FlagBuildableYOffset,
            transform.position.z);
        guiActive = false;
        roadBuidlingGUIActive = false;
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
        if (roadBuidlingGUIActive)
        {

            if (GameHandler.ClickedBuildableFlag != gameObject || GameHandler.CurrentlyBuildingRoad == false || GameHandler.RoadBuildingGUI == null)
            {
                roadBuidlingGUIActive = false;
            }
            else
            {
                GameHandler.RoadBuildingGUI.transform.position = Camera.main.WorldToScreenPoint(transform.position);
            }
        }
    }

    public FlagSkript ReplaceWithFlag()
    {
        GameObject temp = Instantiate(GameHandler.FlagPrefab, transform.position, Quaternion.identity);
        if (guiActive)
        {
            Destroy(gui);
            GameHandler.GUIActive = false;
        }

        Destroy(gameObject);

        return temp.GetComponent<FlagSkript>();
    }

    private void OnMouseDown()
    {
        if (GameHandler.CurrentlyBuildingRoad)
        {
            GameHandler.ClickedBuildableFlag = gameObject;
            GameHandler.CurrentRoad.add_point(transform.position);
            GameHandler.RoadBuildingGUI.transform.position = Camera.main.WorldToScreenPoint(transform.position);
            roadBuidlingGUIActive = true;
        }
        else if (!GameHandler.GUIActive)
        {

            gui = Instantiate(GameHandler.FahnenerzeugungGUIPrefab, Camera.main.WorldToScreenPoint(transform.position) +
                                                                    new Vector3(
                                                                        150, 80, 0), Quaternion.identity,
                GameHandler.MainCanvas.transform);
            guiActive = true;
            GameHandler.GUIActive = true;
            GameHandler.ClickedBuildableFlag = gameObject;
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