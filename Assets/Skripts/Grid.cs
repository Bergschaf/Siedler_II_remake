using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Grid : MonoBehaviour
{
    // Set in Editor
    public LayerMask unwalkableMask;

    // Paramters
    public static Vector2 GridWorldSize;
    public static int GridSizeX, GridSizeY;
    public float nodeRadius;
    private float node_diameter;

    // Grids
    public static Node[,] NodeGrid;
    public Node Paint1, Paint2;

    private void Start()
    {
        // Parameters
        GridWorldSize = new Vector2(GameHandler.ActiveTerrainTerrainData.size.x,
            GameHandler.ActiveTerrainTerrainData.size.z);
        node_diameter = nodeRadius * 2;
        GridSizeX = Mathf.RoundToInt(GridWorldSize.x / node_diameter);
        GridSizeY = Mathf.RoundToInt(GridWorldSize.y / node_diameter);

        // Grid Creation
        CreateGrid();
    }

    void CreateGrid()
    {
        NodeGrid = new Node[GridSizeX, GridSizeY];

        Vector3 worldBottomLeft = transform.position; // Grid Game Object has to be on the bottom left of the terrain 
        //    transform.position - Vector3.right * GridWorldSize.x / 2 - Vector3.forward * GridWorldSize.y / 2;
        Quaternion rotation = Quaternion.Euler(0, 0, 0);
        for (int x = 0; x < GridSizeX; x++)
        {
            for (int y = 0; y < GridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * node_diameter + nodeRadius) +
                                     Vector3.forward * (y * node_diameter + nodeRadius);
                // Check if the point is obstructed or not
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                NodeGrid[x, y] = new Node(walkable, worldPoint, x, y);
                if (walkable)
                {
                    // TODO Calculate where what building size can go
                    Instantiate(GameHandler.BuildableFlag, worldPoint, rotation);
                }

            }
        }
    }

    // Get Neighbours around a Node in a 3x3 Grid
    public static List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < GridSizeX && checkY >= 0 && checkY < GridSizeY)
                {
                    neighbours.Add(NodeGrid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    // Get the node for a World Position
    public static Node NodeFromWorldPoint(Vector3 worldPos)
    {
        float percentX = (worldPos.x) / GridWorldSize.x;
        float percentY = (worldPos.z) / GridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((GridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((GridSizeY - 1) * percentY);
        return NodeGrid[x, y];
    }
    
}