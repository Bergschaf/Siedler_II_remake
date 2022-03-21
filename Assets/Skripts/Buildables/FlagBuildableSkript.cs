using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

/// <summary>
/// Script for the buildable Flag GameObject TODO Change buildable Flag to UI icon
/// </summary>
public class FlagBuildableSkript : MonoBehaviour
{
    /// <summary>
    /// Is the mouse currently over the icon
    /// </summary>
    private bool _mouseOver;
    /// <summary>
    /// The GUI GameObject if the flag menu is open TODO Migrate this to UIHandler
    /// </summary>
    private GameObject _gui;
    /// <summary>
    /// Is the GUI currently active (at this icon)
    /// </summary>
    private bool _guiActive;
    /// <summary>
    /// Is the road building GUI currently active (at this icon)
    /// </summary>
    private bool _roadBuildingGUIActive;

    void Start()
    {
        _mouseOver = false;
        // The position is updated, so every buildable flag has the same distance to the ground
        transform.position = new Vector3(transform.position.x,
            GameHandler.ActiveTerrain.SampleHeight(transform.position) + GameHandler.FlagBuildableYOffset,
            transform.position.z); 
        _guiActive = false;
        _roadBuildingGUIActive = false;
    }

    private void Update()
    {
        if (_guiActive)
        {
            if (_gui != null)
            {
                _gui.transform.position = Camera.main.WorldToScreenPoint(transform.position) + new Vector3(
                    150, 80, 0);
            }
            else
            {
                _guiActive = false;
            }
        }
        if (_roadBuildingGUIActive)
        {

            if (UIHandler.ClickedBuildableFlag != gameObject || GameHandler.CurrentlyBuildingRoad == false || UIHandler.RoadBuildingGUI == null)
            {
                _roadBuildingGUIActive = false;
            }
            else
            {
                UIHandler.RoadBuildingGUI.transform.position = Camera.main.WorldToScreenPoint(transform.position);
            }
        }
    }

    public FlagSkript ReplaceWithFlag()
    {
        GameObject temp = Instantiate(GameHandler.FlagPrefab, transform.position, Quaternion.identity);
        if (_guiActive)
        {
            Destroy(_gui);
            UIHandler.GUIActive = false;
        }

        Destroy(gameObject);

        return temp.GetComponent<FlagSkript>();
    }

    private void OnMouseDown()
    {
        if (GameHandler.CurrentlyBuildingRoad)
        {
            UIHandler.ClickedBuildableFlag = gameObject;
            GameHandler.CurrentRoad.add_point(transform.position);
            UIHandler.RoadBuildingGUI.transform.position = Camera.main.WorldToScreenPoint(transform.position);
            _roadBuildingGUIActive = true;
        }
        else if (!UIHandler.GUIActive)
        {

            _gui = Instantiate(UIHandler.FahnenerzeugungGUIPrefab, Camera.main.WorldToScreenPoint(transform.position) +
                                                                   new Vector3(
                                                                       150, 80, 0), Quaternion.identity,
                UIHandler.MainCanvas.transform);
            _guiActive = true;
            UIHandler.GUIActive = true;
            UIHandler.ClickedBuildableFlag = gameObject;
        }
    }

    private void OnMouseEnter()
    {
        if (!_mouseOver)
        {
            transform.localScale += new Vector3(1, 1, 1);
            _mouseOver = true;
        }
    }

    private void OnMouseExit()
    {
        if (_mouseOver)
        {
            transform.localScale -= new Vector3(1, 1, 1);
            _mouseOver = false;
        }
    }
}