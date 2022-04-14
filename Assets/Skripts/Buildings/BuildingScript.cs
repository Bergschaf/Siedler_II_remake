using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour
{
    public Vector3[] corners; //0 = bottom left, 2 = top right

    public int buildingID;
    void Start()
    {
        UpdateTerrainHeight();
    }
    
    private void UpdateTerrainHeight()
    {
        int[,] terrainCoordinateCorners = new int[2, 2];

        for (int i = 0; i < 2; i++)
        {
            terrainCoordinateCorners[i, 0] = Mathf.RoundToInt(corners[i].x / GameHandler.ActiveTerrainTerrainData.size.x * GameHandler.ActiveTerrainTerrainData.heightmapResolution);
            terrainCoordinateCorners[i, 1] = Mathf.RoundToInt(corners[i].z / GameHandler.ActiveTerrainTerrainData.size.z * GameHandler.ActiveTerrainTerrainData.heightmapResolution);
        }

        float[,] heights = GameHandler.ActiveTerrainTerrainData.GetHeights(terrainCoordinateCorners[0, 0], terrainCoordinateCorners[0, 1], terrainCoordinateCorners[1, 0] - terrainCoordinateCorners[0, 0], terrainCoordinateCorners[1, 1] - terrainCoordinateCorners[0, 1]);
        float heightSum = 0;
        foreach (var h in heights)
        {
            heightSum += h;
        }

        float avgHeight = heightSum / heights.Length;
        
        for (int i = 0; i < heights.GetLength(0); i++)
        {
            for (int j = 0; j < heights.GetLength(1); j++)
            {
                heights[i, j] = avgHeight;
            }
        }   
        GameHandler.ActiveTerrainTerrainData.SetHeightsDelayLOD(terrainCoordinateCorners[0, 0], terrainCoordinateCorners[0, 1], heights);
        GameHandler.ActiveTerrainTerrainData.SyncHeightmap();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
