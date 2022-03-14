using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagSpawnButtonSkript : MonoBehaviour
{
    void click()
    {
        GameHandler.ClickedBuildableFlag.GetComponent<FlagBuildableSkript>().ReplaceWithFlag();
    }
}