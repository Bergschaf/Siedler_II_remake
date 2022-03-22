using System;
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
    public Vector3[] RoadPoints,_roadPointsRight,_roadPointsLeft;
    /// <summary>
    /// Road GameObject
    /// </summary>
    private GameObject _road;
    /// <summary>
    /// Road mesh
    /// </summary>
    private RoadMesh _roadMesh;
    /// <summary>
    /// Start and end position of the road
    /// </summary>
    public Vector3 Pos1,Pos2;
    /// <summary>
    /// Length of the road
    /// </summary>
    public float Len;
    /// <summary>
    /// The exact middle position of the road, where the assigned settler stands TODO remove bugs
    /// </summary>
    public Vector3 MiddlePos; 


    public Road(Vector3[] _road_points, Vector3 _pos1, Vector3 _pos2)
    {
        // Initialisation
        RoadPoints = _road_points;
        Pos1 = _pos1; // First Point of the Road
        Pos2 = _pos2; // last Point of the Road
        for (int i = 1; i < RoadPoints.Length; i++)
        {
            Len += Vector3.Distance(RoadPoints[i - 1], RoadPoints[i]);
        }

        _road = Object.Instantiate(GameHandler.DirtRoadPrefab, Vector3.zero, Quaternion.identity);
        _roadMesh = _road.GetComponent<RoadMesh>();
    }

    
    /// <summary>
    /// Adds a point to the road
    /// </summary>
    /// <param name="point"></param>
    public void add_point(Vector3 point)
    {
        List<Node> path = RoadPathfinding.FindPath(Pos2, point);

        Vector3[] temp = new Vector3[path.Count - 1];


        for (int i = 1; i < path.Count; i++)
        {
            temp[i-1] = path[i].WorldPosition;
            Len += Vector3.Distance(path[i-1].WorldPosition, path[i].WorldPosition);

        }
        RoadPoints = RoadPoints.Concat(temp).ToArray();
        
        Pos2 = point;
        MiddlePos = Vector3.Lerp(RoadPoints[Mathf.FloorToInt((RoadPoints.Length - 1) / 2)],
            RoadPoints[Mathf.CeilToInt((RoadPoints.Length - 1) / 2)], 0.5f);

        draw_road();
    }

    /// <summary>
    /// Lays a smooth curve through the road points, offsets the points to both sides and applies them to the mesh
    /// </summary>
    private void draw_road()
    {
        Vector3[] tempRoadPoints = GameHandler.MakeSmoothCurve(RoadPoints);
        _roadPointsLeft = new Vector3[tempRoadPoints.Length - 2];
        _roadPointsRight = new Vector3[tempRoadPoints.Length - 2];

        for (int i = 1; i < tempRoadPoints.Length - 1; i++)
        {
            float angle = (Mathf.Rad2Deg * Mathf.Atan2(tempRoadPoints[i - 1].x - tempRoadPoints[i].x,
                tempRoadPoints[i - 1].z - tempRoadPoints[i].z));

            Vector3 pos1 = tempRoadPoints[i] +
                           Quaternion.AngleAxis(angle + 90, Vector3.up) * Vector3.forward *
                           GameHandler.RoadWidth / 2;
            Vector3 pos2 = tempRoadPoints[i] +
                           Quaternion.AngleAxis(angle + 90, Vector3.up) * Vector3.forward *
                           GameHandler.RoadWidth / -2;


            _roadPointsLeft[i - 1] = new Vector3(pos1.x, GameHandler.ActiveTerrain.SampleHeight(pos1) + 0.1f, pos1.z);
            _roadPointsRight[i - 1] = new Vector3(pos2.x, GameHandler.ActiveTerrain.SampleHeight(pos2) + 0.1f, pos2.z);
            ;
        }

        _roadMesh.SetVertices(_roadPointsLeft, _roadPointsRight);
    }
    
    /// <summary>
    /// Destroys the road and the corresponding mesh
    /// </summary>
    public void destroy()
    {
        _roadMesh.destroy();
    }
}