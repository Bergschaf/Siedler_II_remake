using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoadMesh : MonoBehaviour
{
    private Mesh _mesh;
    private Vector2[] _uvMap;
    private int[] _triangles;
    

    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<Renderer>();
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        _mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vleft = {new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(3, 0, 0)};
        Vector3[] vright = {new Vector3(0, 1, -1), new Vector3(1, 1, -1), new Vector3(3, 1, -1)};
        SetVertices(vleft, vright);
        GetComponent<Renderer>().material = GameHandler.RoadMaterial;

    }

    public void SetVertices(Vector3[] verticesLeft, Vector3[] verticesRight)
    {
        // TODO Calculate UVS Based on the space a triangel takes 
        _mesh.Clear();
        _mesh.vertices = verticesLeft.Concat(verticesRight).ToArray();

        int verticesLen = verticesLeft.Length;
        _triangles = new int[(verticesLen * 2 - 2) * 3];

        for (int i = 0; i < verticesLen - 1; i ++)
        {
            _triangles[i * 6] = i;
            _triangles[i * 6 + 1] = i + 1;
            _triangles[i * 6 + 2] = i + verticesLen;

            _triangles[i * 6 + 5] = i + verticesLen;
            _triangles[i * 6 + 4] = i + verticesLen + 1;
            _triangles[i * 6 + 3] = i + 1;
        }

        _uvMap = new Vector2[verticesLen * 2];

        for (int i = 0; i < verticesLen; i++)
        {
            _uvMap[i] = new Vector2(0, i % 2);
            _uvMap[i + verticesLen] = new Vector2(1, i % 2);

            
        }

        _mesh.triangles = _triangles;
        _mesh.uv = _uvMap;
    }
}