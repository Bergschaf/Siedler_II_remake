using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for the Button that aborts the road build
/// </summary>
public class AbortRoadBuildButtonScript : MonoBehaviour
{
    public void click()
    {
        GameHandler.EndBuildingRoad(successful:false);
    }
}
