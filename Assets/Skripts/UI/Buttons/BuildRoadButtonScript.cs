using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for the road building button
/// </summary>
public class BuildRoadButtonScript : MonoBehaviour
{
    public void Click(GameObject parentGUI)
    {
        GameHandler.StartBuildingRoad(UIHandler.LastClickedFlag.transform.position);
        Destroy(parentGUI);
    }
}
