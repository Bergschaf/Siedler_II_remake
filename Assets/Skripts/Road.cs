using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// Road class
/// </summary>
public class Road
{
    /// <summary>
    /// Points of the road
    /// </summary>
    public Vector3[] RoadPoints, _roadPointsRight, _roadPointsLeft, SmoothRoadPoints;

    /// <summary>
    /// Road GameObject
    /// </summary>
    private GameObject _road;

    /// <summary>
    /// The visible road
    /// </summary>
    private RoadMesh _roadMesh;

    /// <summary>
    /// Start and end position of the road
    /// </summary>
    public Vector3 Pos1, Pos2;

    /// <summary>
    /// Length of the road
    /// </summary>
    public float Len;

    /// <summary>
    /// The exact middle position of the road, where the assigned settler stands TODO remove bugs
    /// </summary>
    public Vector3 MiddlePos;

    /// <summary>
    /// The Nodes of the road points
    /// </summary>
    public List<Node> Nodes;

    /// <summary>
    /// The Settler that is Assigned to this road
    /// </summary>
    public SettlerScript Settler;
    
    /// <summary>
    /// The Flags at the end of the road
    /// </summary>
    public FlagScript Flag1, Flag2;


    public Road(Vector3 pos1)
    {
        // Initialisation
        RoadPoints = new[] {pos1};
        Pos1 = pos1; // First Point of the Road
        Pos2 = pos1; // last Point of the Road

        _road = Object.Instantiate(GameHandler.DirtRoadPrefab, Vector3.zero, Quaternion.identity);
        _roadMesh = _road.GetComponent<RoadMesh>();
        Nodes = new List<Node> {Grid.NodeFromWorldPoint(Pos1)};
    }


    /// <summary>
    /// Adds a point to the road
    /// </summary>
    /// <param name="point"></param>
    public bool add_point(Vector3 point)
    {
        bool end = Grid.NodeFromWorldPoint(point).Type ==
                   "Road"; // True if the road goes into another road and therefore should be ended
        FlagScript flag = null;
        if (end)
        {
            flag = Grid.NodeFromWorldPoint(point).BuildableIcon.GetComponent<FlagBuildableScript>().ReplaceWithFlag();
        }

        List<Node> path = RoadPathfinding.FindPath(Pos2, point);

        if (path.Count < 2)
        {
            return false;
        }

        for (int i = 1; i < path.Count; i++)
        {
            path[i].Buildable = false;

            if (path[i].Type != "Flag")
            {
                path[i].Type = "Road";
            }

            path[i].Road = this;


            Nodes.Add(path[i]);
        }


        Vector3[] temp = new Vector3[path.Count - 1];


        for (int i = 1; i < path.Count; i++)
        {
            temp[i - 1] = path[i].WorldPosition;
            Len += Vector3.Distance(path[i - 1].WorldPosition, path[i].WorldPosition);
        }

        RoadPoints = RoadPoints.Concat(temp).ToArray();

        Pos2 = point;

        draw_road();

        if (end)
        {
            GameHandler.EndBuildingRoad(flag);
        }

        return true;
    }

    /// <summary>
    /// Lays a smooth curve through the road points, offsets the points to both sides and applies them to the mesh
    /// </summary>
    private void draw_road()
    {
        SmoothRoadPoints = GameHandler.MakeSmoothCurve(RoadPoints);

        // Middle Position of SmoothRoadPoints in MiddlePos

        MiddlePos = SmoothRoadPoints[SmoothRoadPoints.Length / 2];
        

        _roadPointsLeft = new Vector3[SmoothRoadPoints.Length];
        _roadPointsRight = new Vector3[SmoothRoadPoints.Length];

        float angle = (Mathf.Rad2Deg * Mathf.Atan2(SmoothRoadPoints[0].x - SmoothRoadPoints[1].x,
            SmoothRoadPoints[0].z - SmoothRoadPoints[1].z));

        Vector3 pos1 = SmoothRoadPoints[0] +
                       Quaternion.AngleAxis(angle + 90, Vector3.up) * Vector3.forward *
                       GameHandler.RoadWidth / 2;
        Vector3 pos2 = SmoothRoadPoints[0] +
                       Quaternion.AngleAxis(angle + 90, Vector3.up) * Vector3.forward *
                       GameHandler.RoadWidth / -2;
        _roadPointsLeft[0] = new Vector3(pos1.x, GameHandler.ActiveTerrain.SampleHeight(pos1) + 0.1f, pos1.z);
        _roadPointsRight[0] = new Vector3(pos2.x, GameHandler.ActiveTerrain.SampleHeight(pos2) + 0.1f, pos2.z);

        for (int i = 1; i < SmoothRoadPoints.Length; i++)
        {
            angle = (Mathf.Rad2Deg * Mathf.Atan2(SmoothRoadPoints[i - 1].x - SmoothRoadPoints[i].x,
                SmoothRoadPoints[i - 1].z - SmoothRoadPoints[i].z));

            pos1 = SmoothRoadPoints[i] +
                   Quaternion.AngleAxis(angle + 90, Vector3.up) * Vector3.forward *
                   GameHandler.RoadWidth / 2;
            pos2 = SmoothRoadPoints[i] +
                   Quaternion.AngleAxis(angle + 90, Vector3.up) * Vector3.forward *
                   GameHandler.RoadWidth / -2;


            _roadPointsLeft[i] = new Vector3(pos1.x, GameHandler.ActiveTerrain.SampleHeight(pos1) + 0.1f, pos1.z);
            _roadPointsRight[i] = new Vector3(pos2.x, GameHandler.ActiveTerrain.SampleHeight(pos2) + 0.1f, pos2.z);
        }

        _roadMesh.SetVertices(_roadPointsLeft, _roadPointsRight);
    }


    public void EndRoadBuild()
    {
        bool destroyed = false;
        for (int i = 1; i < Nodes.Count - 1; i++)
        {
            if (Nodes[i].Type == "Flag")
            {
                if(GameHandler.PlaceFlagInRoad(Nodes[i].Flag)) {
                }
            }
        }
        SettlerHandler.OnRoadPlacement(this);
        Flag1 = Grid.NodeFromWorldPoint(Pos1).Flag;
        Flag2 = Grid.NodeFromWorldPoint(Pos2).Flag;

    }


    /// <summary>
    /// Destroys the road and the corresponding mesh
    /// </summary>
    public void destroy()
    {
        for (int i = 0; i < Nodes.Count; i++)
        {
            Nodes[i].Road = null;

            Nodes[i].CalculateBuildableType();
        }

        if (Settler != null) Settler.GoBackToHomeFlag();

        _roadMesh.destroy();
    }
}