using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for the flag spawning button
/// </summary>
public class FlagSpawnButtonSkript : MonoBehaviour
{
    public void click()
    {
        FlagSkript tempflag = UIHandler.ClickedBuildableFlag.GetComponent<FlagBuildableSkript>().ReplaceWithFlag();
        GameHandler.EndBuildingRoad(tempflag);
    }
}