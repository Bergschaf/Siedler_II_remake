using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class RoadMesh : MonoBehaviour
{
    private Mesh _mesh;
    private Vector2[] _uvMap;
    private int[] _triangles;


    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        _mesh = GetComponent<MeshFilter>().mesh;

        GetComponent<MeshRenderer>().material = GameHandler.DirtRoadMaterial;
    }

    public void SetVertices(Vector3[] verticesLeft, Vector3[] verticesRight)
    {
        // TODO Calculate UVS Based on the space a triangle takes 
        
        
        
        _mesh.Clear();
        _mesh.vertices = verticesLeft.Concat(verticesRight).ToArray();


        int verticesLen = verticesLeft.Length;
        _triangles = new int[(verticesLen * 2 - 2) * 3];

        for (int i = 0; i < verticesLen - 1; i++)
        {
            // 0  1  2
            // 3  4  5
            _triangles[i * 6] = i;
            _triangles[i * 6 + 1] = i + 1;
            _triangles[i * 6 + 2] = i + verticesLen;

            _triangles[i * 6 + 5] = i + verticesLen;
            _triangles[i * 6 + 4] = i + verticesLen + 1;
            _triangles[i * 6 + 3] = i + 1;
        }

        _uvMap = new Vector2[verticesLen * 2];
        float count = 0;
        bool count_up = true;
        for (int i = 0; i < verticesLen; i++)
        {
            
            _uvMap[i] = new Vector2(0, count);
            _uvMap[i + verticesLen] = new Vector2(1, count);

            if (count_up)
            {
                count += 0.5f;

            }
            else
            {
                count -= 0.5f;
            }
            if(count >= 1)
            {
                count_up = false;
            }
            else if(count <= 0)
            {
                count_up = true;
            }

        }

        _mesh.triangles = _triangles;
        _mesh.uv = _uvMap;
        _mesh.RecalculateNormals();
    }

    public void destroy()
    {
        Destroy(gameObject);
    }
}