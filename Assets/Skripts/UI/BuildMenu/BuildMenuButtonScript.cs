using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMenuButtonScript : MonoBehaviour
{
    public int buttonID;
    
    public void Click()
    {
        if (buttonID == -1)
        {
            Grid.NodeFromWorldPoint(UIHandler.ClickedBuildable.transform.position).BuildableIcon
                .GetComponent<BuildableScript>().ReplaceWithFlag();
            return;
        }
        Node newFlagNode = Grid.NodeGrid[Grid.NodeFromWorldPoint(UIHandler.ClickedBuildable.transform.position).GridX, Grid.NodeFromWorldPoint(UIHandler.ClickedBuildable.transform.position).GridY - 1];
        FlagScript flag;

        if (newFlagNode.Flag == null)
        {
            newFlagNode.BuildableIcon.GetComponent<BuildableScript>().ReplaceWithFlag();
        }

        flag = newFlagNode.Flag;
        flag.PlaceBuildingAtFlag(buttonID);
        UIHandler.EndGUI();
    }
}
