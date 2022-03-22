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

    // Variables


    /// <summary>
    /// Is any GUI window active
    /// </summary>
    public static bool GUIActive;

    /// <summary>
    /// The last clicked buildable Flag TODO change when buildable Flags becomes UI
    /// </summary>
    public static GameObject ClickedBuildableFlag; // Important for RoadBuilding

    /// <summary>
    /// The last clicked Flag
    /// </summary>
    public static GameObject LastClickedFlag;

    /// <summary>
    /// The main GameObject of the GUI windows, if active
    /// </summary>
    public static GameObject MainGUIWindow;

    /// <summary>
    /// The world position where the currently active GUI is
    /// </summary>
    public static Vector3 MainGUIWorldPos;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (GUIActive)
        {
            MainGUIWindow.transform.position = Camera.main.WorldToScreenPoint(MainGUIWorldPos);
        }
    }

    // Specific GUI methods

    /// <summary>
    /// Initialise the road building GUI
    /// </summary>
    /// <param name="worldPos">the world position of the road building GUI</param>
    public static void StartRoadBuildingGUI(Vector3 worldPos)
    {
        MainGUIWindow = Instantiate(StraßenbaumenuGUIPrefab, Camera.main.WorldToScreenPoint(worldPos),
            Quaternion.identity, MainCanvas.transform);
        GUIActive = true;
        MainGUIWorldPos = worldPos;
    }

    public static void UpdateGUIWorldPos(Vector3 worldPos)
    {
        MainGUIWorldPos = worldPos;
    }


    /// <summary>
    /// Destroy the current active GUI
    /// </summary>
    public static void EndGUI()
    {
        GUIActive = false;
        Destroy(MainGUIWindow);
    }

    /// <summary>
    /// Initialise the Flag menu GIU
    /// </summary>
    /// <param name="worldPos"></param>
    public static void StartFlagGUI(Vector3 worldPos)
    {
        MainGUIWindow = Instantiate(FahnenmenuPrefab, Camera.main.WorldToScreenPoint(worldPos),
            Quaternion.identity, MainCanvas.transform);
        GUIActive = true;
        MainGUIWorldPos = worldPos;
    }
    
    /// <summary>
    /// Initialise the Flag menu GIU
    /// </summary>
    /// <param name="worldPos"></param>
    public static void StartFlagCreationGUI(Vector3 worldPos)
    {
        MainGUIWindow = Instantiate(FahnenerzeugungGUIPrefab, Camera.main.WorldToScreenPoint(worldPos),
            Quaternion.identity, MainCanvas.transform);
        GUIActive = true;
        MainGUIWorldPos = worldPos;
    }
}