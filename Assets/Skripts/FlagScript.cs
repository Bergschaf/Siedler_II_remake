using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
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


    private void Awake()
    {
        Vector3 position = transform.position;
        position = new Vector3(position.x,
            GameHandler.ActiveTerrain.SampleHeight(position),
            position.z);
        transform.position = position;

        Node temp = Grid.NodeFromWorldPoint(position);
        
        if (temp.Type == "Road")
        {
            GameHandler.PlaceFlagInRoad(this);
        }

        temp.Type = "Flag";
        
        temp.Flag = this;
        temp.BuildableIcon.SetActive(false);
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

    private void DestroyDirtCrossing()
    {
        _dirtCrossing.GetComponent<DirtCrossingMeshScript>().destroy();
        GameHandler.AllFlags.Remove(this);
    }

    public void AddRoad(Road road, FlagScript targetFlagScript)
    {
        if (AttachedRoads == null)
        {
            AttachedRoads = new List<Tuple<Road, FlagScript>>();
        }

        if (AttachedRoads.Count < 1)
        {
            GenerateDirtCrossing();
        }

        AttachedRoads.Add(new Tuple<Road, FlagScript>(road, targetFlagScript));
    }

    public void RemoveRoad(Road road)
    {
        foreach (var t in AttachedRoads)
        {
            if (t.Item1 == road)
            {
                AttachedRoads.Remove(t);
                t.Item1.destroy();
                break;
            }
        }

        if (AttachedRoads.Count < 1)
        {
            DestroyDirtCrossing();
        }
    }

    /// <summary>
    /// Destroys the Flag and removes all the road connections
    /// </summary>
    public void Destroy()
    {   
        
        // The Roads that should be removed from a flag, because they connect to this flag
        List<Road> toRemove;
        if (AttachedRoads != null)
            foreach (var t in AttachedRoads)
            {
                if (t.Item2.AttachedRoads != null)

                {
                    toRemove = new List<Road>();
                    foreach (var x in t.Item2.AttachedRoads)
                    {
                        if (x.Item2 == this)
                        {
                            toRemove.Add(x.Item1);
                        }
                    }

                    foreach (var r in toRemove)
                    {
                        t.Item2.RemoveRoad(r);
                    }
                }
            }

        GameHandler.AllFlags.Remove(this);
        DestroyDirtCrossing();
        Destroy(gameObject);
        UIHandler.EndGUI();
        var position = transform.position;
        Grid.NodeFromWorldPoint(position).Type = "Buildable";
        Grid.NodeFromWorldPoint(position).CalculateBuildableType();
    }
}