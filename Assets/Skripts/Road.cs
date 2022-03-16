using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using UnityEditor;
using UnityEngine;

public class Road
{
    // Paramters
    public Vector3[] RoadPoints;
    private Vector3[] _roadPointsRight;
    private Vector3[] _roadPointsLeft;
    private GameObject _road;
    private RoadMesh _roadMesh;
    public Vector3 Pos1;
    public Vector3 Pos2;
    public float Len;


    public Road(Vector3[] _road_points, Vector3 _pos1, Vector3 _pos2)
    {
        // Initialisation
        RoadPoints = _road_points;
        Pos1 = _pos1;
        Pos2 = _pos2;
        for (int i = 1; i < RoadPoints.Length; i++)
        {
            Len += Vector3.Distance(RoadPoints[i - 1], RoadPoints[i]);
        }

        _road = GameObject.Instantiate(GameHandler.RoadPrefab);
        _roadMesh = _road.GetComponent<RoadMesh>();
    }

    public void add_point(Vector3 point)
    {
        List<Node> path = Road_Pathfinding.FindPath(Pos2, point);

        Vector3[] temp = new Vector3[RoadPoints.Length + path.Count];
        for (int i = 0; i < RoadPoints.Length; i++)
        {
            temp[i] = RoadPoints[i];
        }

        for (int i = 0; i < path.Count; i++)
        {
            temp[i + RoadPoints.Length] = path[i].worldPosition;
        }

        RoadPoints = temp;
        Len += Vector3.Distance(Pos2, point);

        Pos2 = point;

        draw_road();
    }

    private void draw_road()
    {
        _roadPointsLeft = new Vector3[RoadPoints.Length - 1];
        _roadPointsRight = new Vector3[RoadPoints.Length - 1];

        for (int i = 0; i < RoadPoints.Length - 1; i++)
        {
            float angle = Mathf.Rad2Deg * Mathf.Atan2(RoadPoints[i - 1].x - RoadPoints[i].x,
                RoadPoints[i - 1].z - RoadPoints[i].z);
            Vector3 pos1 = RoadPoints[i] +
                           Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward *
                           GameHandler.RoadWidth / 2;
            Vector3 pos2 = RoadPoints[i] +
                           Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward *
                           GameHandler.RoadWidth / -2;
            _roadPointsLeft[i] = pos1;
            _roadPointsRight[i] = pos2;
        }

        _roadMesh.SetVertices(_roadPointsLeft, _roadPointsRight);
    }
}