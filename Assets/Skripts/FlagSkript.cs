using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The script assigned to the flag GameObject
/// </summary>
public class FlagSkript : MonoBehaviour
{
    
    /// <summary>
    /// The GameObject of the crossing at the road
    /// </summary>
    private GameObject _dirtCrossing;
    
    /// <summary>
    /// All road attached to the Flag
    /// </summary>
    /// <code>List(Tuple(Road,FlagScript): Road,TargetFlag</code>
    public List<Tuple<Road, FlagSkript>> AttachedRoads; // (Road,Target)

    // Start is called before the first frame update
    void Start()
    {
        Vector3 position = transform.position;
        position = new Vector3(position.x,
            GameHandler.ActiveTerrain.SampleHeight(position),
            position.z);
        transform.position = position;
    }

    private void OnMouseDown()
    {
        if (GameHandler.CurrentlyBuildingRoad)
        {
            GameHandler.CurrentRoad.add_point(transform.position);
            GameHandler.EndBuildingRoad(gameObject.GetComponent<FlagSkript>());
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
        _dirtCrossing.GetComponent<DirtCrossingMeshSkript>().SetVertices(transform.position);
        if (!GameHandler.AllFlags.Contains(this))
        {
            GameHandler.AllFlags.Add(this);
        }
    }

    public void AddRoad(Road road, FlagSkript targetFlagSkript)
    {
        GenerateDirtCrossing();
        if (AttachedRoads == null)
        {
            AttachedRoads = new List<Tuple<Road, FlagSkript>>();
        }

        AttachedRoads.Add(new Tuple<Road, FlagSkript>(road, targetFlagSkript));
    }
}