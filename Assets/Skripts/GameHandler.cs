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

    // Terrain
    public Terrain activeTerrain;

    // Textures
    public Texture roadTexture;
    
    // Building Paramters
    public float flagBuildableYOffset;
    
    // GUI Prefabs
    public GameObject fahnenerzeugungGUIPrefab;
    public GameObject straßenbaumenuGUIPrefab;
    
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

    // Terrain
    public static Terrain ActiveTerrain;
    public static TerrainData ActiveTerrainTerrainData;

    // Textures
    public static Texture RoadTexture;
    
    // Building Parameters
    public static float FlagBuildableYOffset;
    
    // GUI Prefabs
    public static GameObject FahnenerzeugungGUIPrefab;
    public static GameObject StraßenbaumenuGUIPrefab;
    
    // GUI
    public static Canvas MainCanvas;
    public static bool GUIActive;
    
    // Variables
    public static GameObject ClickedBuildableFlag;
    

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
        
        // Terrain
        ActiveTerrain = activeTerrain;
        ActiveTerrainTerrainData = ActiveTerrain.terrainData;

        // Textures
        RoadTexture = roadTexture;
        
        // Building Parameters
        FlagBuildableYOffset = flagBuildableYOffset;
        
        // GUI Prefabs
        FahnenerzeugungGUIPrefab = fahnenerzeugungGUIPrefab;
        StraßenbaumenuGUIPrefab = straßenbaumenuGUIPrefab;
        
        // GUI
        MainCanvas = mainCanvas;
        GUIActive = false;
    }
}