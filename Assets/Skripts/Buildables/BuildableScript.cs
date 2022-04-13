using UnityEngine;

/// <summary>
/// Script for the buildable Flag GameObject TODO Change buildable Flag to UI icon
/// </summary>
public class BuildableScript : MonoBehaviour
{

    /// <summary>
    /// 0: Flag, 1: small house, 2: medium house, 3: large house
    /// </summary>
    public int buildableType;

    void Start()
    {
        // The position is updated, so every buildable flag has the same distance to the ground
        var position = transform.position;
        position = new Vector3(position.x,
            GameHandler.ActiveTerrain.SampleHeight(position) + GameHandler.FlagBuildableYOffset,
            position.z);
        transform.position = position;
    }


    public FlagScript ReplaceWithFlag()
    {
        GameObject temp = Instantiate(GameHandler.FlagPrefab, transform.position, Quaternion.identity);
        UIHandler.EndGUI();

        return temp.GetComponent<FlagScript>();
    }

    private void OnMouseDown()
    {
        if (GameHandler.CurrentlyBuildingRoad)
        {
            var position = transform.position;


            if (GameHandler.CurrentRoad.add_point(position))
            {
                UIHandler.ClickedBuildableFlag = gameObject;
                UIHandler.UpdateGUIWorldPos(position);
            }
        }
        else if (!UIHandler.GUIActive)
        {
            UIHandler.StartFlagCreationGUI(transform.position);
            UIHandler.GUIActive = true;
            UIHandler.ClickedBuildableFlag = gameObject;
        }
    }

    private void OnMouseEnter()
    {
        if(buildableType == 0)
        {
            transform.localScale += new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
        }
        
    }

    private void OnMouseExit()
    {
        if(buildableType == 0)
        {
            transform.localScale -= new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale -= new Vector3(0.2f, 0.2f, 0.2f);
        }
        
    }
}