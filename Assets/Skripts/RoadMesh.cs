using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Custom road mesh on top of the terrain
/// </summary>
public class RoadMesh : MonoBehaviour
{
    /// <summary>
    /// The road Mesh
    /// </summary>
    private Mesh _mesh;
    /// <summary>
    /// The UV-Map for the mesh
    /// </summary>
    private Vector2[] _uvMap;
    /// <summary>
    /// The array of triangles of the mesh
    /// </summary>
    private int[] _triangles;


    // Start is called before the first frame update
    void Awake()
    {
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        _mesh = GetComponent<MeshFilter>().mesh;

        GetComponent<MeshRenderer>().material = GameHandler.DirtRoadMaterial;
    }

    /// <summary>
    /// Applies the points of the left and right array to the mesh
    /// </summary>
    /// <param name="verticesLeft">the points of one side of the road</param>
    /// <param name="verticesRight">the points of the other side of the road</param>
    public void SetVertices(Vector3[] verticesLeft, Vector3[] verticesRight)
    {
        // TODO Calculate UVS Based on the space a triangle takes 

        _mesh.Clear();
        _mesh.vertices = verticesLeft.Concat(verticesRight).ToArray();


        int verticesLen = verticesLeft.Length;
        _triangles = new int[(verticesLen * 2 - 2) * 3];

        for (int i = 0; i < verticesLen - 1; i++)
        {
            _triangles[i * 6] = i;
            _triangles[i * 6 + 1] = i + 1;
            _triangles[i * 6 + 2] = i + verticesLen;

            _triangles[i * 6 + 5] = i + verticesLen;
            _triangles[i * 6 + 4] = i + verticesLen + 1;
            _triangles[i * 6 + 3] = i + 1;
        }

        _uvMap = new Vector2[verticesLen * 2];
        float count = 0;
        bool countUp = true;
        for (int i = 0; i < verticesLen; i++)
        {
            
            _uvMap[i] = new Vector2(0, count);
            _uvMap[i + verticesLen] = new Vector2(1, count);

            if (countUp)
            {
                count += 0.5f;

            }
            else
            {
                count -= 0.5f;
            }
            if(count >= 1)
            {
                countUp = false;
            }
            else if(count <= 0)
            {
                countUp = true;
            }

        }

        _mesh.triangles = _triangles;
        _mesh.uv = _uvMap;
        _mesh.RecalculateNormals();
    }

    /// <summary>
    /// Destroys the mesh
    /// </summary>
    public void destroy()
    {
        if (gameObject != null) Destroy(gameObject);
    }
}