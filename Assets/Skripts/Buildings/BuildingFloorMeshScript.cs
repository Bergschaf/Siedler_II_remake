using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingFloorMeshScript : MonoBehaviour
{

    private Mesh _mesh;

    private Vector2[] _uvMap;

    private int[] _triangles;

    public int buildingID;

    void Awake()
    {
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        _mesh = GetComponent<MeshFilter>().mesh;

        GetComponent<MeshRenderer>().material = GameHandler.DirtRoadMaterial;
    }
    
    
    
    
}
