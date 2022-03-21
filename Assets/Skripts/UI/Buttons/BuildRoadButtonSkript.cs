using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildRoadButtonSkript : MonoBehaviour
{
    public void Click(GameObject parentGUI)
    {
        GameHandler.StartBuildingRoad(UIHandler.LastClickedFlag.transform.position);
        Destroy(parentGUI);
    }
}
