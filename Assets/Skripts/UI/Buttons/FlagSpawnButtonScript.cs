using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for the flag spawning button
/// </summary>
public class FlagSpawnButtonScript : MonoBehaviour
{
    public void click()
    {
        FlagScript tempflag = UIHandler.ClickedBuildableFlag.GetComponent<BuildableScript>().ReplaceWithFlag();
        GameHandler.EndBuildingRoad(tempflag);
    }
}