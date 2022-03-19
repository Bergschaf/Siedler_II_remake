using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettlerSkript : MonoBehaviour
{
    public string job;
    private float _speed = 10;
    public Road AssignedRoad;
    public Vector3[] pathToTravel;
    public FlagSkript currentFlag;
    private float _interpolation;
    private float _interpolationStepSize;
    private int _pathToTravelIndex; // where in that path is the settler right now

    // Start is called before the first frame update
    void Start()
    {
        job = "Standart";
        currentFlag = GameHandler.HomeFlag;
    }

    public void AssignRoad(FlagSkript flag, Road roadToAssign)
    {
        Road[] roadPath = GameHandler.GetRoadGridPath(flag, currentFlag);
        List<Vector3> tempPath = new List<Vector3>();
        foreach (var road in roadPath)
        {
            if (road == roadToAssign)
            {
                if (Vector3.Distance(roadToAssign.Pos1, flag.transform.position) <
                    Vector3.Distance(roadToAssign.Pos2, flag.transform.position))
                {
                    for (int i = 0; i < road.RoadPoints.Length / 2; i++)
                    {
                        tempPath.Add(road.RoadPoints[i]);
                    }
                }
                else
                {
                    for (int i = Mathf.RoundToInt(road.RoadPoints.Length / 2); i > -1; i--)
                    {
                        tempPath.Add(road.RoadPoints[i]);
                    }
                }
            }
            else
            {
                if (Vector3.Distance(road.Pos1, tempPath[tempPath.Count - 1]) <
                    Vector3.Distance(road.Pos2, tempPath[tempPath.Count - 1]))
                {
                    for (int i = 0; i < road.RoadPoints.Length; i++)
                    {
                        tempPath.Add(road.RoadPoints[i]);
                    }
                }
                else
                {
                    for (int i =  road.RoadPoints.Length - 1; i > -1; i--)
                    {
                        tempPath.Add(road.RoadPoints[i]);
                    }
                }
            }
        }
        pathToTravel = tempPath.ToArray();

        _interpolationStepSize =
            Vector3.Distance(pathToTravel[0], pathToTravel[1]) / _speed *
            Time.deltaTime;
        _pathToTravelIndex = 0;
        _interpolation = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (pathToTravel.Length > 0)
        {
            if (_interpolation >= 1)
            {
                _interpolation = 0;
                _pathToTravelIndex += 1;
                if (_pathToTravelIndex + 2 >= pathToTravel.Length)
                {
                    pathToTravel = Array.Empty<Vector3>();
                    _pathToTravelIndex = 0;
                    return;
                }

                _interpolationStepSize =
                    Vector3.Distance(pathToTravel[_pathToTravelIndex], pathToTravel[_pathToTravelIndex + 1]) / _speed *
                    Time.deltaTime;
            }

            Vector3 tempPos =Vector3.Lerp(pathToTravel[_pathToTravelIndex], pathToTravel[_pathToTravelIndex + 1],
                _interpolation);
            transform.position = new Vector3(tempPos.x, GameHandler.ActiveTerrain.SampleHeight(tempPos), tempPos.z);
            
            _interpolation += _interpolationStepSize;
        }
    }

    private void OnMouseDown()
    {
        AssignRoad(GameHandler.AllFlags[GameHandler.AllFlags.Count - 1 ],GameHandler.AllFlags[GameHandler.AllFlags.Count - 1 ].AttachedRoads[0].Item1);
    }
}