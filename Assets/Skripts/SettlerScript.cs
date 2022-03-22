using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for each settler, not fully developed yet
/// </summary>
public class SettlerScript : MonoBehaviour
{
    /// <summary>
    /// The settlers job
    /// </summary>
    public string job;
    /// <summary>
    /// The walking speed of the settler
    /// </summary>
    private float _speed = 10;
    /// <summary>
    /// The road the settler should stand at and carry the Items between the two flags of the road
    /// </summary>
    public Road AssignedRoad;
    /// <summary>
    /// The path the settler has to walk
    /// </summary>
    public Vector3[] pathToTravel;
    /// <summary>
    /// The flag the settler is currently at
    /// </summary>
    public FlagScript currentFlag;
    /// <summary>
    /// Variables for travelling over multiple frames
    /// </summary>
    private float _interpolation,_interpolationStepSize;
    /// <summary>
    /// At what position in the path array is the settler right now
    /// </summary>
    private int _pathToTravelIndex;

    // Start is called before the first frame update
    void Start()
    {
        job = "Standard";
        currentFlag = GameHandler.HomeFlag;
    }

    /// <summary>
    /// Sets the road a settler should stand at and carry the items between the two flags of the road
    /// </summary>
    /// <param name="flag">a flag of the road</param>
    /// <param name="roadToAssign">the road the settler should go to</param>
    public void AssignRoad(FlagScript flag, Road roadToAssign)
    {
        Road[] roadPath = GameHandler.GetRoadGridPath(currentFlag, flag);
        List<Vector3> tempPath = new List<Vector3>();
        tempPath.Add(currentFlag.transform.position);
        Vector3[] smoothRoadPoints;

        foreach (var road in roadPath)
        {
            smoothRoadPoints = GameHandler.MakeSmoothCurve(road.RoadPoints);
            if (road == roadToAssign)
            {
                if (Vector3.Distance(roadToAssign.Pos1, tempPath[tempPath.Count - 1]) <
                    Vector3.Distance(roadToAssign.Pos2, tempPath[tempPath.Count - 1]))
                {
                    for (int i = 0; i < Mathf.RoundToInt(smoothRoadPoints.Length / 2); i++)
                    {
                        tempPath.Add(smoothRoadPoints[i]);
                    }
                }

                else
                {
                    for (int i = smoothRoadPoints.Length - 1; i > Mathf.RoundToInt(smoothRoadPoints.Length / 2); i--)
                    {
                        tempPath.Add(smoothRoadPoints[i]);
                    }
                }

                tempPath.Add(road.MiddlePos);
            }
            else
            {
                if (Vector3.Distance(road.Pos1, tempPath[tempPath.Count - 1]) <
                    Vector3.Distance(road.Pos2, tempPath[tempPath.Count - 1]))
                {
                    for (int i = 0; i < smoothRoadPoints.Length; i++)
                    {
                        tempPath.Add(smoothRoadPoints[i]);
                    }
                }
                else
                {
                    for (int i = smoothRoadPoints.Length - 1; i > -1; i--)
                    {
                        tempPath.Add(smoothRoadPoints[i]);
                    }
                }
            }
        }

        pathToTravel = tempPath.ToArray();

        _interpolationStepSize =
            _speed / Vector3.Distance(pathToTravel[0], pathToTravel[1]);

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
                    _speed / Vector3.Distance(pathToTravel[_pathToTravelIndex], pathToTravel[_pathToTravelIndex + 1]);
            }

            Vector3 tempPos = Vector3.Lerp(pathToTravel[_pathToTravelIndex], pathToTravel[_pathToTravelIndex + 1],
                _interpolation);


            transform.position = new Vector3(tempPos.x, GameHandler.ActiveTerrain.SampleHeight(tempPos), tempPos.z);

            _interpolation += _interpolationStepSize * Time.deltaTime;
        }
    }

    private void OnMouseDown()
    {
        AssignRoad(GameHandler.AllFlags[GameHandler.AllFlags.Count - 1],
            GameHandler.AllFlags[GameHandler.AllFlags.Count - 1].AttachedRoads[0].Item1);
    }
}