using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handle the UI Things
/// </summary>
public class UIHandler : MonoBehaviour
{

    // Set in Inspector
    
    // GUI Prefabs
    /// <summary>
    /// The prefab of the flag spawning menu
    /// </summary>
    public GameObject fahnenerzeugungGUIPrefab;
    /// <summary>
    /// The prefab of the road building menu
    /// </summary>
    public GameObject straßenbaumenuGUIPrefab;
    /// <summary>
    /// The prefab of the flag menu
    /// </summary>
    public GameObject fahenmenuPrefab;

    // GUI
    public Canvas mainCanvas;

    // Static Variables
    
    // GUI Prefabs
    public static GameObject FahnenerzeugungGUIPrefab;
    public static GameObject StraßenbaumenuGUIPrefab;
    public static GameObject FahnenmenuPrefab;

    // GUI
    
    public static Canvas MainCanvas;
    /// <summary>
    /// Is any GUI window active
    /// </summary>
    public static bool GUIActive;
    /// <summary>
    /// Is the road building GUI active
    /// </summary>
    public static bool RoadBuildingGUIActive;

    // Variables
    
    /// <summary>
    /// The last clicked buildable Flag TODO change when buildable Flags becomes UI
    /// </summary>
    public static GameObject ClickedBuildableFlag; // Important for RoadBuilding
    /// <summary>
    /// The last clicked Flag
    /// </summary>
    public static GameObject LastClickedFlag;
    /// <summary>
    /// The main GameObject of teh road building GUI
    /// </summary>
    public static GameObject RoadBuildingGUI;
    /// <summary>
    /// The world Position where the road building gui currently is
    /// </summary>
    public static Vector3 RoadBuildingGUIWorldPos;

    // Start is called before the first frame update
    void Awake()
    {
        // GUI Prefabs
        FahnenerzeugungGUIPrefab = fahnenerzeugungGUIPrefab;
        StraßenbaumenuGUIPrefab = straßenbaumenuGUIPrefab;
        FahnenmenuPrefab = fahenmenuPrefab;

        // GUI
        MainCanvas = mainCanvas;
        GUIActive = false;
        RoadBuildingGUIActive = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (RoadBuildingGUIActive)
        {
            RoadBuildingGUI.transform.position = Camera.main.WorldToScreenPoint(RoadBuildingGUIWorldPos);
        }
    }

    /// <summary>
    /// Initiate the road building GUI
    /// </summary>
    /// <param name="worldPos">the world position of the road building GUI</param>
    public static void StartRoadBuildingGUI(Vector3 worldPos)
    {
        RoadBuildingGUI = Instantiate(StraßenbaumenuGUIPrefab, Camera.main.WorldToScreenPoint(worldPos),
            Quaternion.identity, MainCanvas.transform);
        GUIActive = true;
        RoadBuildingGUIActive = true;
        RoadBuildingGUIWorldPos = worldPos;
    }


    /// <summary>
    /// Destroy the road building GUI
    /// </summary>
    public static void EndRoadBuildingGUI()
    {
        GUIActive = false;
        RoadBuildingGUIActive = false;
        Destroy(RoadBuildingGUI);
    }
}