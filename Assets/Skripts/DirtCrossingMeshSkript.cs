using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtCrossingMeshSkript : MonoBehaviour
{
    private Mesh _mesh;
    private Vector3[] vertices;
    private int num_vertices = 36; // has to be even
    private float radius = 3f;
    private Vector2[] _uvMap;
    private int[] _triangles;


    // Start is called before the first frame update
    void Awake()
    {
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        _mesh = GetComponent<MeshFilter>().mesh;
        GetComponent<MeshRenderer>().material = GameHandler.DirtCrossingMaterial;
    }

    public void SetVertices(Vector3 middlePoint)
    {
        vertices = new Vector3[num_vertices + 1];

        middlePoint = new Vector3(middlePoint.x, GameHandler.ActiveTerrain.SampleHeight(middlePoint) + 0.1f,
            middlePoint.z);
        vertices[0] = middlePoint;
        int c = 1;
        for (float i = 0; i < 360; i += 360/num_vertices)
        {
            Vector3 pos = middlePoint +
                          Quaternion.AngleAxis(i, Vector3.up) * Vector3.forward *
                          radius;

            vertices[c] = new Vector3(pos.x, GameHandler.ActiveTerrain.SampleHeight(pos) + 0.1f, pos.z);
            c++;
        }
        
        _mesh.Clear();
        _mesh.vertices = vertices;


        int verticesLen = vertices.Length;
        _triangles = new int[verticesLen * 3];

        for (int i = 0; i < verticesLen - 1; i++)
        {
            // 0  1  2
            // 3  4  5
            _triangles[i * 3] = 0;
            _triangles[i * 3 + 1] = i;
            _triangles[i * 3 + 2] = i + 1;
            

        }

        _triangles[verticesLen * 3 - 3] = 0;
        _triangles[verticesLen * 3 - 2] = verticesLen -1;
        _triangles[verticesLen * 3 - 1] = 1;


        _uvMap = new Vector2[verticesLen];
        Vector3 dif;
        _uvMap[0] = new Vector2(0.5f, 0.5f);
        for (int i = 1; i < verticesLen; i++)
        {
            dif =  middlePoint - vertices[i];
            
            _uvMap[i] = new Vector2(dif.x/ (radius * 2) + 0.5f, dif.z/ (radius * 2) + 0.5f);
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