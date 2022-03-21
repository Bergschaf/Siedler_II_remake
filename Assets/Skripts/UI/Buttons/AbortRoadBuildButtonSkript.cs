using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbortRoadBuildButtonSkript : MonoBehaviour
{
    public void click()
    {
        GameHandler.EndBuildingRoad(successful:false);
    }
}
