using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHandler : MonoBehaviour
{
    // Set in inspector
    public GameObject[] buildingPrefabs;
    
    public GameObject buildingFloorPrefab;
    
    // Static
    public static GameObject[] BuildingPrefabs;
    
    public static GameObject BuildingFloorPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        BuildingPrefabs = buildingPrefabs;
        BuildingFloorPrefab = buildingFloorPrefab;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
