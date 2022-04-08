using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    private float _interpolation, _interpolationStepSize;

    /// <summary>
    /// At what position in the path array is the settler right now
    /// </summary>
    private int _pathToTravelIndex;

    // Start is called before the first frame update
    void Awake()
    {
        job = "Standard";
        currentFlag = GameHandler.HomeFlag;
    }

    /// <summary>
    /// Sets the road a settler should stand at and carry the items between the two flags of the road
    /// </summary>
    /// <param name="roadToAssign">the road the settler should go to</param>
    /// <param name="roadPath">The path the Settler should take to the road</param>
    public void AssignRoad(Road roadToAssign, Road[] roadPath)
    {
        AssignedRoad = roadToAssign;
        List<Vector3> Path = new List<Vector3>();
        Vector3 lastPos = currentFlag.transform.position;
        foreach (var r in roadPath)
        {
            if(Vector3.Distance(r.Pos1,lastPos) < Vector3.Distance(r.Pos2,lastPos))
            {
                for (int i = 0; i < r.RoadPoints.Length; i++)
                {
                    Path.Add(r.RoadPoints[i]);
                }
                lastPos = r.Pos2;
            }
            else
            {
                for (int i = r.RoadPoints.Length - 1; i >= 0; i--)
                {
                    Path.Add(r.RoadPoints[i]);
                    
                }

                lastPos = r.Pos1;
            }
            
        }
        
        if(Vector3.Distance(roadToAssign.Pos1,lastPos) < Vector3.Distance(roadToAssign.Pos2,lastPos))
        {
            for (int i = 0; i < roadToAssign.RoadPoints.Length / 2; i++)
            {
                Path.Add(roadToAssign.RoadPoints[i]);
            }
        }
        else
        {
            for (int i = roadToAssign.RoadPoints.Length - 1; i >= roadToAssign.RoadPoints.Length / 2; i--)
            {
                Path.Add(roadToAssign.RoadPoints[i]);
                    
            }
        }
        Path.Add(roadToAssign.MiddlePos);

        pathToTravel = GameHandler.MakeSmoothCurve(Path.ToArray());
        
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

    public void GoBackToHomeFlag()
    {
        // TODO Check if settler could go to a nearby road

    }
}