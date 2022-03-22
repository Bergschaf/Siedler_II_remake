using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The script for the closing button of an UI element
/// </summary>
public class ClosingButtonScript : MonoBehaviour
{
    /// <summary>
    /// Called when the button is clicked
    /// </summary>
    /// <param name="parent">The parent GameObject of the Button</param>
    public void click(GameObject parent)
    {
        UIHandler.GUIActive = false;
        GameHandler.EndBuildingRoad(successful:false);
        Destroy(parent);
    }
    }
