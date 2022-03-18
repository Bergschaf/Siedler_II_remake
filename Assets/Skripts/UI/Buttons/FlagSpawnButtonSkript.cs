using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagSpawnButtonSkript : MonoBehaviour
{
    public void click()
    {
        FlagSkript tempflag = GameHandler.ClickedBuildableFlag.GetComponent<FlagBuildableSkript>().ReplaceWithFlag();
        GameHandler.EndBuildingRoad(tempflag);
    }
}