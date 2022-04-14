using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHandler : MonoBehaviour
{
    public GameObject[] buildingPrefabs;
    
    public static GameObject[] BuildingPrefabs;
    
    // Start is called before the first frame update
    void Start()
    {
        BuildingPrefabs = buildingPrefabs;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
