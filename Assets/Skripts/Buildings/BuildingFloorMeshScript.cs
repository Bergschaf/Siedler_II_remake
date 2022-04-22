using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingFloorMeshScript : MonoBehaviour
{

    private Mesh _mesh;

    private Vector2[] _uvMap;

    private int[] _triangles;
    
    void Awake()
    {
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        _mesh = GetComponent<MeshFilter>().mesh;

        GetComponent<MeshRenderer>().material = GameHandler.BuildingFloorMaterial;

    }

    /// <summary>
    /// Creates a mesh for the floor of a building, while it is in construction.
    /// </summary>
    public void DrawBuildingFloor(Vector3[] corners)
    {
        // TODO Dynamic floor size depending on building size
        
        _mesh.Clear();
        
        Vector3[] vertices = 
        {
            new Vector3(corners[0].y, 0, corners[1].x),
            corners[0],
            corners[1],   
            new Vector3(corners[1].y, 0, corners[0].x)
        };

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i].y = GameHandler.ActiveTerrain.SampleHeight(vertices[i]) + 0.5f;
        }

        _mesh.vertices = vertices;
        _mesh.triangles = new int[]
        {
            0, 1, 2,
            3,2,1
        };

        _uvMap = new Vector2[]
        {
            new Vector2(0, 0),
            new Vector2(0.5f, 0),
            new Vector2(0, 0.5f),
            new Vector2(0.5f, 0.5f)
        };
        
        _mesh.uv = _uvMap;
        _mesh.RecalculateNormals();
        
    }
    
    /// <summary>
    /// Destroys the mesh
    /// </summary>
    public void destroy()
    {
        if (gameObject != null) Destroy(gameObject);
        Destroy(this);
    }
    
}
