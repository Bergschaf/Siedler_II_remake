using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class GameHandler : MonoBehaviour
{
    // Variables to set in the untiy editor
    // Buildable Prefabs
    public GameObject buildableFlag;
    public GameObject buildableHouse1;
    public GameObject buildableHouse2;
    public GameObject buildableHouse3;
    public GameObject buildableHarbour;

    // Building Prefabs
    public GameObject flagPrefab;
    public  GameObject roadPrefab;
    
    // Terrain
    public Terrain activeTerrain;

    // Materials
    public Material roadMaterial;
    
    // Building Paramters
    public float flagBuildableYOffset;
    public float roadWidth;
    
    // GUI Prefabs
    public GameObject fahnenerzeugungGUIPrefab;
    public GameObject straßenbaumenuGUIPrefab;
    public GameObject fahenmenuPrefab;
    
    // GUI
    public Canvas mainCanvas;

    // static global variables
    
    // Buildable Prefabs
    public static GameObject BuildableFlag;
    public static GameObject BuildableHouse1;
    public static GameObject BuildableHouse2;
    public static GameObject BuildableHouse3;
    public static GameObject BuildableHarbour;
    
    // Building Prefabs
    public static GameObject FlagPrefab;
    public static GameObject RoadPrefab;

    // Terrain
    public static Terrain ActiveTerrain;
    public static TerrainData ActiveTerrainTerrainData;

    // Materials
    public static Material RoadMaterial;
    
    // Building Parameters
    public static float FlagBuildableYOffset;
    public static float RoadWidth ;
    private static int _numberOfPoints = 10;
    
    // GUI Prefabs
    public static GameObject FahnenerzeugungGUIPrefab;
    public static GameObject StraßenbaumenuGUIPrefab;
    public static GameObject FahnenmenuPrefab;
    
    // GUI
    public static Canvas MainCanvas;
    public static bool GUIActive;
    
    // Variables
    public static GameObject ClickedBuildableFlag; // Important for RoadBuilding
    public static bool CurrentlyBuildingRoad;
    public static GameObject LastClickedFlag;

    // Roads
    public static List<Road> RoadGrid;
    public static Road CurrentRoad;
    
    

    private void Awake()
    {
        // Buildable Prefabs
        BuildableFlag = buildableFlag;
        BuildableHouse1 = buildableHouse1;
        BuildableHouse2 = buildableHouse2;
        BuildableHouse3 = buildableHouse3;
        BuildableHarbour = buildableHarbour;

        // Building Prefabs
        FlagPrefab = flagPrefab;
        RoadPrefab = roadPrefab;
        
        // Terrain
        ActiveTerrain = activeTerrain;
        ActiveTerrainTerrainData = ActiveTerrain.terrainData;

        // Materials
        RoadMaterial = roadMaterial;
        
        // Building Parameters
        FlagBuildableYOffset = flagBuildableYOffset;
        RoadWidth = roadWidth;
        
        // GUI Prefabs
        FahnenerzeugungGUIPrefab = fahnenerzeugungGUIPrefab;
        StraßenbaumenuGUIPrefab = straßenbaumenuGUIPrefab;
        FahnenmenuPrefab = fahenmenuPrefab;
        
        // GUI
        MainCanvas = mainCanvas;
        GUIActive = false;
        
        // Roads
        RoadGrid = new List<Road>();
        CurrentlyBuildingRoad = false;

    }

    public static void StartBuildingRoad(Vector3 position)
    {
        Vector3[] temp = {position};
        CurrentRoad = new Road(temp, position, position);
        CurrentlyBuildingRoad = true;
    }

    public static void EndBuildingRoad(bool from_buildable_flag)
    {
        // from_buildable_flag is this method called from a buildable flag, which has to be replaced by a real flag
        if (from_buildable_flag)
        {
            ClickedBuildableFlag.GetComponent<FlagBuildableSkript>().ReplaceWithFlag();
        }
        RoadGrid.Add(CurrentRoad);
        CurrentlyBuildingRoad = false;
        GUIActive = false;
    }
    
    public static Vector3[] MakeSmoothCurve(Vector3[] points)
    {
        Vector3 p0, p1, m0, m1;
        Vector3[] final_points = new Vector3[_numberOfPoints * (points.Length-1)];
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
            for(int i = 0; i < _numberOfPoints; i++) 
            {
                t = i * pointStep;
                position = (2.0f * t * t * t - 3.0f * t * t + 1.0f) * p0 
                           + (t * t * t - 2.0f * t * t + t) * m0 
                           + (-2.0f * t * t * t + 3.0f * t * t) * p1 
                           + (t * t * t - t * t) * m1;
                final_points[i + j * _numberOfPoints] = position;
            }
        }

        return final_points;
    }
    
}