using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The script assigned to the flag GameObject
/// </summary>
public class FlagScript : MonoBehaviour
{
    /// <summary>
    /// The GameObject of the crossing at the road
    /// </summary>
    private GameObject _dirtCrossing;

    /// <summary>
    /// All road attached to the Flag
    /// </summary>
    /// <code>List(Tuple(Road,FlagScript): Road,TargetFlag</code>
    public List<Tuple<Road, FlagScript>> AttachedRoads; // (Road,Target)

    // Start is called before the first frame update
    void Start()
    {
        Vector3 position = transform.position;
        position = new Vector3(position.x,
            GameHandler.ActiveTerrain.SampleHeight(position),
            position.z);
        transform.position = position;
        
        Node temp = Grid.NodeFromWorldPoint(position);
        if (temp.Type == "Road" && temp.RoadTo.Count > 1)
        {
            GameHandler.PlaceFlagInRoad(this);
        }
        temp.Type = "Flag";
        temp.Flag = this;
        temp.RoadTo = new List<Node>();

    }

    private void OnMouseDown()
    {
        if (GameHandler.CurrentlyBuildingRoad)
        {
            if (GameHandler.CurrentRoad.add_point(transform.position))
            {
                GameHandler.EndBuildingRoad(gameObject.GetComponent<FlagScript>());
            }
        }
        else if (!UIHandler.GUIActive)
        {
            UIHandler.StartFlagGUI(transform.position);
        }

        UIHandler.LastClickedFlag = gameObject;
    }

    private void GenerateDirtCrossing()
    {
        _dirtCrossing = Instantiate(GameHandler.DirtCrossingPrefab);
        _dirtCrossing.GetComponent<DirtCrossingMeshScript>().SetVertices(transform.position);
        if (!GameHandler.AllFlags.Contains(this))
        {
            GameHandler.AllFlags.Add(this);
        }
    }

    public void AddRoad(Road road, FlagScript targetFlagScript)
    {
        GenerateDirtCrossing();
        if (AttachedRoads == null)
        {
            AttachedRoads = new List<Tuple<Road, FlagScript>>();
        }

        AttachedRoads.Add(new Tuple<Road, FlagScript>(road, targetFlagScript));
    }
}