using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The script assigned to the flag GameObject
/// </summary>
public class FlagSkript : MonoBehaviour
{
    private bool _guiActive; // TODO Migrate this to UIHandler

    /// <summary>
    /// The GameObject of the crossing at the road
    /// </summary>
    private GameObject _dirtCrossing;

    private GameObject _gui; // TODO Migrate this to UIHandler

    /// <summary>
    /// All road attached to the Flag
    /// </summary>
    /// <code>List(Tuple(Road,FlagScript): Road,TargetFlag</code>
    public List<Tuple<Road, FlagSkript>> AttachedRoads; // (Road,Target)

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x,
            GameHandler.ActiveTerrain.SampleHeight(transform.position),
            transform.position.z);
    }


    private void Update()
    {
        if (_guiActive)
        {
            if (_gui == null)
            {
                _guiActive = false;
                UIHandler.GUIActive = false;
            }
            else
            {
                _gui.transform.position = Camera.main.WorldToScreenPoint(transform.position) + new Vector3(150, 80, 0);
            }
        }
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
            _gui = Instantiate(UIHandler.FahnenmenuPrefab,
                Camera.main.WorldToScreenPoint(transform.position) + new Vector3(150, 80, 0), Quaternion.identity,
                UIHandler.MainCanvas.transform);
            UIHandler.LastClickedFlag = gameObject;
            _guiActive = true;
            UIHandler.GUIActive = true;
        }

        GameHandler.GetRoadGridPath(this, this); // temp
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