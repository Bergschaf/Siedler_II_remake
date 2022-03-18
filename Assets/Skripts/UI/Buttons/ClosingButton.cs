using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosingButton : MonoBehaviour
{
    public void click(GameObject parent)
    {
        GameHandler.GUIActive = false;
        GameHandler.EndBuildingRoad(succesfull:false);
        Destroy(parent);
    }
    }
