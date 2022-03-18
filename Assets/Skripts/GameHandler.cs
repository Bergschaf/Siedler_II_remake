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
    public GameObject dirtRoadPrefab;
    public GameObject dirtCrossingPrefab;

    // Terrain
    public Terrain activeTerrain;

    // Materials
    public Material dirtRoadMaterial;
    public Material dirtCrossingMaterial;

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
    public static GameObject DirtRoadPrefab;
    public static GameObject DirtCrossingPrefab;

    // Terrain
    public static Terrain ActiveTerrain;
    public static TerrainData ActiveTerrainTerrainData;

    // Materials
    public static Material DirtRoadMaterial;
    public static Material DirtCrossingMaterial;

    // Building Parameters
    public static float FlagBuildableYOffset;
    public static float RoadWidth;
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
    public static GameObject RoadBuildingGUI;
    public static GameObject RoadBuildStartFlag;

    // Roads
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
        DirtRoadPrefab = dirtRoadPrefab;
        DirtCrossingPrefab = dirtCrossingPrefab;

        // Terrain
        ActiveTerrain = activeTerrain;
        ActiveTerrainTerrainData = ActiveTerrain.terrainData;

        // Materials
        DirtRoadMaterial = dirtRoadMaterial;
        DirtCrossingMaterial = dirtCrossingMaterial;

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
        CurrentlyBuildingRoad = false;
    }

    public static void StartBuildingRoad(Vector3 position)
    {
        Vector3[] temp = {position};
        CurrentRoad = new Road(temp, position, position);
        CurrentlyBuildingRoad = true;
        RoadBuildingGUI = Instantiate(StraßenbaumenuGUIPrefab, Camera.main.WorldToScreenPoint(position),
            Quaternion.identity, MainCanvas.transform);
        RoadBuildStartFlag = LastClickedFlag;
    }

    public static void EndBuildingRoad(FlagSkript endFlag = null, bool succesfull = true)
    {

            // from_buildable_flag is this method called from a buildable flag, which has to be replaced by a real flag
            if (succesfull && CurrentlyBuildingRoad && endFlag != null)
            {
                endFlag.AddRoad(CurrentRoad,RoadBuildStartFlag.GetComponent<FlagSkript>());
                RoadBuildStartFlag.GetComponent<FlagSkript>().AddRoad(CurrentRoad,endFlag);
            }
            else
            {
                if (CurrentRoad != null) CurrentRoad.destroy();
            }

            CurrentlyBuildingRoad = false;
            GUIActive = false;
            Destroy(RoadBuildingGUI);
        
    }

    public static Vector3[] MakeSmoothCurve(Vector3[] points)
    {
        Vector3 p0, p1, m0, m1;
        Vector3[] final_points = new Vector3[_numberOfPoints * (points.Length - 1)];
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
                final_points[i + j * _numberOfPoints] = position;
            }
        }

        return final_points;
    }
}