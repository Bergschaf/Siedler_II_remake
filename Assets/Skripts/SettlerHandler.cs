using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SettlerHandler : MonoBehaviour
{
    // Prefabs
    public GameObject settlerPrefab;

    // Static Prefabs
    public static GameObject SettlerPrefab;

    /// <summary>
    /// All the Settlers that are currently in the game
    /// </summary>
    public static List<SettlerScript> AllSettlers;

    /// <summary>
    /// All Roads with no Settler Assigned
    /// </summary>
    public static List<Road> NotAssignedRoads;

    private static List<Road> toRemove;

    void Awake()
    {
        // Prefabs
        SettlerPrefab = settlerPrefab;

        AllSettlers = new List<SettlerScript>();
        NotAssignedRoads = new List<Road>();
    }


    /// <summary>
    /// Called whenever a road is placed
    /// </summary>
    public static void OnRoadPlacement(Road road)
    {
        NotAssignedRoads.Add(road);
        toRemove = new List<Road>();
        foreach (var r in NotAssignedRoads)
        {
            FlagScript flag1 = Grid.NodeFromWorldPoint(r.Pos1).Flag;

            Road[] roadPath = GameHandler.GetRoadGridPath(GameHandler.HomeFlag, flag1);

            Debug.Log(roadPath);

            FlagScript flag2 = Grid.NodeFromWorldPoint(r.Pos2).Flag;
            ;
            if (roadPath == null)
            {
                roadPath = GameHandler.GetRoadGridPath(GameHandler.HomeFlag, flag2);
            }

            Debug.Log(roadPath);

            if (roadPath.Length == 0 && !(flag1 == GameHandler.HomeFlag || flag2 == GameHandler.HomeFlag))
            {
                continue;
            }

            toRemove.Add(r);
            SettlerScript tempSettler = SpawnSettler(GameHandler.HomeFlag.transform.position);
            tempSettler.AssignRoad(r, roadPath);
        }

        foreach (var r in toRemove)
        {
            NotAssignedRoads.Remove(r);
        }
    }

    /// <summary>
    /// Returns a new Settler at the specified Position
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public static SettlerScript SpawnSettler(Vector3 position)
    {
        SettlerScript tempSettler =
            Instantiate(SettlerPrefab, position, Quaternion.identity).GetComponent<SettlerScript>();
        AllSettlers.Add(tempSettler);
        return tempSettler;
    }
}