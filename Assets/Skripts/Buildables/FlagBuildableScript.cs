using UnityEngine;

/// <summary>
/// Script for the buildable Flag GameObject TODO Change buildable Flag to UI icon
/// </summary>
public class FlagBuildableScript : MonoBehaviour
{
    /// <summary>
    /// Is the mouse currently over the icon
    /// </summary>
    private bool _mouseOver;

    void Start()
    {
        _mouseOver = false;
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
        Destroy(gameObject);
        
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
        if (!_mouseOver)
        {
            transform.localScale += new Vector3(1, 1, 1);
            _mouseOver = true;
        }
    }

    private void OnMouseExit()
    {
        if (_mouseOver)
        {
            transform.localScale -= new Vector3(1, 1, 1);
            _mouseOver = false;
        }
    }
}