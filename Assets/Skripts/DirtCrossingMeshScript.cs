using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtCrossingMeshScript : MonoBehaviour
{
    /// <summary>
    /// The mesh of the crossing
    /// </summary>
    private Mesh _mesh;
    /// <summary>
    /// The vertices of the mesh
    /// </summary>
    private Vector3[] _vertices;
    /// <summary>
    /// The number of vertices around the center point
    /// </summary>
    private int num_vertices = 36;
    /// <summary>
    /// The radius of the crossing
    /// </summary>
    private float radius = 3f;
    /// <summary>
    /// The UV-Map of the crossing
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
        GetComponent<MeshRenderer>().material = GameHandler.DirtCrossingMaterial;
    }
    
    /// <summary>
    /// Set a middle point of the crossing
    /// </summary>
    /// <param name="middlePoint">Middle point of the crossing</param>
    public void SetVertices(Vector3 middlePoint)
    {
        _vertices = new Vector3[num_vertices + 1];

        middlePoint = new Vector3(middlePoint.x, GameHandler.ActiveTerrain.SampleHeight(middlePoint) + 0.1f,
            middlePoint.z);
        _vertices[0] = middlePoint;
        int c = 1;
        for (float i = 0; i < 360; i += 360 / num_vertices)
        {
            Vector3 pos = middlePoint +
                          Quaternion.AngleAxis(i, Vector3.up) * Vector3.forward *
                          radius;

            _vertices[c] = new Vector3(pos.x, GameHandler.ActiveTerrain.SampleHeight(pos) + 0.1f, pos.z);
            c++;
        }

        _mesh.Clear();
        _mesh.vertices = _vertices;


        int verticesLen = _vertices.Length;
        _triangles = new int[verticesLen * 3];

        for (int i = 0; i < verticesLen - 1; i++)
        {
            _triangles[i * 3] = 0;
            _triangles[i * 3 + 1] = i;
            _triangles[i * 3 + 2] = i + 1;
        }

        _triangles[verticesLen * 3 - 3] = 0;
        _triangles[verticesLen * 3 - 2] = verticesLen - 1;
        _triangles[verticesLen * 3 - 1] = 1;


        _uvMap = new Vector2[verticesLen];
        Vector3 dif;
        _uvMap[0] = new Vector2(0.5f, 0.5f);
        for (int i = 1; i < verticesLen; i++)
        {
            dif = middlePoint - _vertices[i];

            _uvMap[i] = new Vector2(dif.x / (radius * 2) + 0.5f, dif.z / (radius * 2) + 0.5f);
        }

        _mesh.triangles = _triangles;
        _mesh.uv = _uvMap;
        _mesh.RecalculateNormals();
    }

    /// <summary>
    /// Destroys the crossing mesh
    /// </summary>
    public void destroy()
    {
        Destroy(gameObject);
    }
}