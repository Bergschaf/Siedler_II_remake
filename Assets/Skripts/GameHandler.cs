using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class GameHandler : MonoBehaviour
{
    public static GameHandler Instance; // required for coroutines

    // Variables to set in the untiy editor
    // Buildable Prefabs
    public GameObject buildableFlag;
    public GameObject buildableHouse1;
    public GameObject buildableHouse2;
    public GameObject buildableHouse3;
    public GameObject buildableHarbour;

    // Building Prefabs
    /// <summary>
    /// Prefab of the flag GameObject
    /// </summary>
    public GameObject flagPrefab;

    /// <summary>
    /// Prefab of the dirt road GameObject
    /// </summary>
    public GameObject dirtRoadPrefab;

    /// <summary>
    /// Prefab of the dirt crossing GameObject
    /// </summary>
    public GameObject dirtCrossingPrefab;


    // Terrain
    /// <summary>
    /// The main terrain
    /// </summary>
    public Terrain activeTerrain;

    public LayerMask terrainLayer;

    // Materials
    public Material dirtRoadMaterial;
    public Material dirtCrossingMaterial;

    // Building Parameters

    public float flagBuildableYOffset;

    /// <summary>
    /// The width of the standard dirt roads
    /// </summary>
    public float roadWidth;


    // static global variables

    // Buildable Prefabs
    public static GameObject BuildableFlag;
    public static GameObject BuildableHouse1;
    public static GameObject BuildableHouse2;
    public static GameObject BuildableHouse3;
    public static GameObject BuildableHarbour;

    // Building Prefabs
    public static GameObject FlagPrefab;
    public static GameObject DirtRoadPrefab;
    public static GameObject DirtCrossingPrefab;

    // Terrain
    public static Terrain ActiveTerrain;
    public static TerrainData ActiveTerrainTerrainData;
    public static LayerMask TerrainLayer;

    // Materials
    public static Material DirtRoadMaterial;
    public static Material DirtCrossingMaterial;

    // Building Parameters
    public static float FlagBuildableYOffset;
    public static float RoadWidth;

    /// <summary>
    /// The number of points of the smooth curve
    /// </summary>
    private static int _numberOfPoints = 10;

    // Variables
    /// <summary>
    /// All flags with roads attached
    /// </summary>
    public static List<FlagScript> AllFlags;

    /// <summary>
    /// The flag where the road building process was started
    /// </summary>
    public static GameObject RoadBuildStartFlag;

    // Roads
    /// <summary>
    /// The road that is currently being built
    /// </summary>
    public static Road CurrentRoad;

    /// <summary>
    /// True if a road is currently being built
    /// </summary>
    public static bool CurrentlyBuildingRoad;

    // Settlers
    /// <summary>
    /// The flag of the main building where the settlers spawn
    /// </summary>
    public static FlagScript HomeFlag;

    private void Awake()
    {
        Instance = this;

        // Buildable Prefabs
        BuildableFlag = buildableFlag;
        BuildableHouse1 = buildableHouse1;
        BuildableHouse2 = buildableHouse2;
        BuildableHouse3 = buildableHouse3;
        BuildableHarbour = buildableHarbour;

        // Building Prefabs
        FlagPrefab = flagPrefab;
        DirtRoadPrefab = dirtRoadPrefab;
        DirtCrossingPrefab = dirtCrossingPrefab;

        // Terrain
        ActiveTerrain = activeTerrain;
        ActiveTerrainTerrainData = ActiveTerrain.terrainData;
        TerrainLayer = terrainLayer;

        // Materials
        DirtRoadMaterial = dirtRoadMaterial;
        DirtCrossingMaterial = dirtCrossingMaterial;

        // Building Parameters
        FlagBuildableYOffset = flagBuildableYOffset;
        RoadWidth = roadWidth;

        // Variables
        AllFlags = new List<FlagScript>();

        // Roads
        CurrentlyBuildingRoad = false;
    }

    private void Start()
    {
        HomeFlag = Grid.NodeGrid[1, 1].BuildableIcon.GetComponent<FlagBuildableScript>().ReplaceWithFlag();
    }

    /// <summary>
    /// Starts a road building process
    /// </summary>
    /// <param name="position">The starting point of the road building process</param>
    public static void StartBuildingRoad(Vector3 position)
    {
        CurrentlyBuildingRoad = true;
        CurrentRoad = new Road(position); // Instantiate a road
        RoadBuildStartFlag = UIHandler.LastClickedFlag;
        UIHandler.StartRoadBuildingGUI(position); // Start the GUI
    }

    /// <summary>
    /// Ends the road building process
    /// </summary>
    /// <param name="endFlag">The flag where the building process ended</param>
    /// <param name="successful">Is the process finished or aborted</param>
    public static void EndBuildingRoad(FlagScript endFlag = null, bool successful = true)
    {
        if (successful && CurrentlyBuildingRoad && endFlag != null)
        {
            // The road is added to the flags
            endFlag.AddRoad(CurrentRoad, RoadBuildStartFlag.GetComponent<FlagScript>());
            RoadBuildStartFlag.GetComponent<FlagScript>().AddRoad(CurrentRoad, endFlag);
            CurrentRoad.EndRoadBuild();
        }
        else
        {
            // If the building process is aborted, the road is destroyed
            if (CurrentRoad != null && CurrentlyBuildingRoad) CurrentRoad.destroy();
        }

        CurrentlyBuildingRoad = false;
        // The GUI gets destroyed
        UIHandler.EndGUI();
    }

    /// <summary>
    /// Places a flag at a node which is already road
    /// </summary>
    /// <param name="flag">the flag to be placed</param>
    public static bool PlaceFlagInRoad(FlagScript flag)
    {
        Vector3 flagPos = flag.transform.position;
        Node node = Grid.NodeFromWorldPoint(flagPos);
        Road road = node.Road;

        FlagScript flag1 = Grid.NodeFromWorldPoint(road.Pos1).Flag;
        FlagScript flag2 = Grid.NodeFromWorldPoint(road.Pos2).Flag;

        if (flag1 == null || flag2 == null)
        {
            return false;
        }

        flag1.RemoveRoad(road);
        flag2.RemoveRoad(road);

        int splitIndex = 0;
        float dist = Vector3.Distance(road.RoadPoints[0], flagPos);
        for (int i = 1; i < road.RoadPoints.Length; i++)
        {
            if (Vector3.Distance(road.RoadPoints[i], flagPos) < dist)
            {
                dist = Vector3.Distance(road.RoadPoints[i], flagPos);
                splitIndex = i;
            }
        }

        Vector3[] roadPoints = road.RoadPoints;

        road.destroy();
        Road newRoad1 = new Road(roadPoints[0]);
        for (int i = 1; i < splitIndex + 1; i++)
        {
            newRoad1.add_point(roadPoints[i]);
        }

        Road newRoad2 = new Road(roadPoints[splitIndex]);
        for (int i = splitIndex + 1; i < roadPoints.Length; i++)
        {
            newRoad2.add_point(roadPoints[i]);
        }

        flag1.AddRoad(newRoad1, flag);
        flag.AddRoad(newRoad1, flag1);

        flag2.AddRoad(newRoad2, flag);
        flag.AddRoad(newRoad2, flag2);

        newRoad1.EndRoadBuild();
        newRoad2.EndRoadBuild();
        return true;
    }


    /// <summary>
    /// Lay a smooth curve through the points (Copied from the unity forum)
    /// </summary>
    /// <param name="points">Array of the points the curve should go through</param>
    /// <returns>Array of points resembling the smooth curve</returns>
    public static Vector3[] MakeSmoothCurve(Vector3[] points)
    {
        Vector3 p0, p1, m0, m1;
        Vector3[] finalPoints = new Vector3[_numberOfPoints * (points.Length - 1)];
        for (int j = 0; j < points.Length - 1; j++)
        {
            // determine control points of segment
            p0 = points[j];
            p1 = points[j + 1];

            if (j > 0)
            {
                m0 = 0.5f * (points[j + 1]
                             - points[j - 1]);
            }
            else
            {
                m0 = points[j + 1]
                     - points[j];
            }

            if (j < points.Length - 2)
            {
                m1 = 0.5f * (points[j + 2]
                             - points[j]);
            }
            else
            {
                m1 = points[j + 1]
                     - points[j];
            }

            // set points of Hermite curve
            Vector3 position;
            float t;
            float pointStep = 1.0f / _numberOfPoints;

            if (j == points.Length - 2)
            {
                pointStep = 1.0f / (_numberOfPoints - 1.0f);
                // last point of last segment should reach p1
            }

            for (int i = 0; i < _numberOfPoints; i++)
            {
                t = i * pointStep;
                position = (2.0f * t * t * t - 3.0f * t * t + 1.0f) * p0
                           + (t * t * t - 2.0f * t * t + t) * m0
                           + (-2.0f * t * t * t + 3.0f * t * t) * p1
                           + (t * t * t - t * t) * m1;
                finalPoints[i + j * _numberOfPoints] = position;
            }
        }

        return finalPoints;
    }

    // Road Graph Variables
    /// <summary>
    /// Distance of the nodes (flags) from the starting Node
    /// </summary>
    public static Dictionary<FlagScript, float> Distance;

    /// <summary>
    /// The parent node (flag) of each flag;
    /// </summary>
    public static Dictionary<FlagScript, FlagScript> Parent;

    /// <summary>
    /// Calculates a path on the available roads using the Dijkstra algorithm TODO optimize the function
    /// </summary>
    /// <param name="startPos">Starting flag of the path</param>
    /// <param name="targetPos">Ending flag of the path</param>
    /// <returns>Array of roads from the start flag to the target flag, empty if no path is found</returns>
    public static Road[] GetRoadGridPath(FlagScript startPos, FlagScript targetPos)
    {
        FlagScript currentFlag;
        List<FlagScript> qList = AllFlags.ToList();

        Parent = new Dictionary<FlagScript, FlagScript>();
        Distance = new Dictionary<FlagScript, float>();
        foreach (var v in AllFlags)
        {
            Distance[v] = float.PositiveInfinity;
            Parent[v] = null;
        }

        Distance[startPos] = 0;
        while (qList.Count > 0)
        {
            currentFlag = qList[0];
            foreach (var k in qList)
            {
                if (Distance[k] < Distance[currentFlag])
                {
                    currentFlag = k;
                }
            }

            if (currentFlag == targetPos)
            {
                List<Road> path = new List<Road>();
                FlagScript u = targetPos;
                foreach (var x in u.AttachedRoads)
                {
                    if (x.Item2 == Parent[u])
                    {
                        path.Add(x.Item1);
                        break;
                    }
                }

                while (Parent[u] != null)
                {
                    u = Parent[u];
                    foreach (var x in u.AttachedRoads)
                    {
                        if (x.Item2 == Parent[u])
                        {
                            path.Add(x.Item1);
                            break;
                        }
                    }
                }

                path.Reverse();
                return path.ToArray();
            }

            qList.Remove(currentFlag);

            foreach (var n in currentFlag.AttachedRoads)
            {
                if (qList.Contains(n.Item2))
                {
                    float newDist = Distance[currentFlag] + n.Item1.Len;
                    if (newDist < Distance[n.Item2])
                    {
                        Distance[n.Item2] = newDist;
                        Parent[n.Item2] = currentFlag;
                    }
                }
            }
        }

        return null;
    }


    /// <summary>
    /// Calculates a path from the starting Position to each target Position on the available roads using the Dijkstra algorithm TODO optimize the function
    /// </summary>
    /// <param name="startPos">Starting flag of the path</param>
    /// <param name="targetPositions">Target Flags of the path</param>
    /// <returns>List of Road paths from the start position to each target position</returns>
    public static List<Road[]> GetMultipleRoadGridPaths(FlagScript startPos, FlagScript[] targetPositions)
    {
        List<Road[]> paths = new List<Road[]>();
        for (int i = 0; i < targetPositions.Length; i++)
        {
            paths.Add(Array.Empty<Road>());
        }

        FlagScript currentFlag;
        List<FlagScript> qList = AllFlags.ToList();

        Parent = new Dictionary<FlagScript, FlagScript>();
        Distance = new Dictionary<FlagScript, float>();
        foreach (var v in AllFlags)
        {
            Distance[v] = float.PositiveInfinity;
            Parent[v] = null;
        }

        Distance[startPos] = 0;
        while (qList.Count > 0)
        {
            currentFlag = qList[0];
            foreach (var k in qList)
            {
                if (Distance[k] < Distance[currentFlag])
                {
                    currentFlag = k;
                }
            }

            for (int i = 0; i < targetPositions.Length; i++)
            {
                if (currentFlag == targetPositions[i])
                {
                    List<Road> path = new List<Road>();
                    FlagScript u = targetPositions[i];
                    foreach (var x in u.AttachedRoads)
                    {
                        if (x.Item2 == Parent[u])
                        {
                            path.Add(x.Item1);
                            break;
                        }
                    }

                    while (Parent[u] != null)
                    {
                        u = Parent[u];
                        foreach (var x in u.AttachedRoads)
                        {
                            if (x.Item2 == Parent[u])
                            {
                                path.Add(x.Item1);
                                break;
                            }
                        }
                    }

                    path.Reverse();
                    paths[i] = path.ToArray();
                }
            }


            qList.Remove(currentFlag);

            foreach (var n in currentFlag.AttachedRoads)
            {
                if (qList.Contains(n.Item2))
                {
                    float newDist = Distance[currentFlag] + n.Item1.Len;
                    if (newDist < Distance[n.Item2])
                    {
                        Distance[n.Item2] = newDist;
                        Parent[n.Item2] = currentFlag;
                    }
                }
            }
        }

        return paths;
    }


    /// <summary>
    /// Executes all coroutines in the coroutine queue
    /// </summary>
    /// <param name="coroutines"></param>
    /// <returns></returns>
    public static IEnumerator ExecuteCoroutines(List<IEnumerator> coroutines)
    {
        foreach (var coroutine in coroutines)
        {
            yield return Instance.StartCoroutine(coroutine);
        }
    }
}