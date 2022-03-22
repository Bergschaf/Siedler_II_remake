using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// The main grid of nodes
/// </summary>
public class Grid : MonoBehaviour
{
    /// <summary>
    /// The unbuildable Layer 
    /// </summary>
    public LayerMask unbuildableMask;

    // Paramters
    /// <summary>
    /// The world size of the Grid
    /// </summary>
    public static Vector2 GridWorldSize;
    /// <summary>
    /// The size of the grid (in grid units)
    /// </summary>
    public static int GridSizeX, GridSizeY;
    /// <summary>
    /// The radius of a node
    /// </summary>
    public float nodeRadius;
    /// <summary>
    /// The diameter of a node
    /// </summary>
    private float _nodeDiameter;

    /// <summary>
    /// The main node grid
    /// </summary>
    public static Node[,] NodeGrid;

    private void Start()
    {
        // Parameters
        GridWorldSize = new Vector2(GameHandler.ActiveTerrainTerrainData.size.x,
            GameHandler.ActiveTerrainTerrainData.size.z);
        _nodeDiameter = nodeRadius * 2;
        GridSizeX = Mathf.RoundToInt(GridWorldSize.x / _nodeDiameter);
        GridSizeY = Mathf.RoundToInt(GridWorldSize.y / _nodeDiameter);

        // Grid Creation
        CreateGrid();
    }
    
    /// <summary>
    /// Initializes the node grid
    /// </summary>
    void CreateGrid()
    {
        NodeGrid = new Node[GridSizeX, GridSizeY];

        Vector3 worldBottomLeft = transform.position; // Grid Game Object has to be on the bottom left of the terrain 
        Quaternion rotation = Quaternion.Euler(0, 0, 0);
        for (int x = 0; x < GridSizeX; x++)
        {
            for (int y = 0; y < GridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * _nodeDiameter + nodeRadius) +
                                     Vector3.forward * (y * _nodeDiameter + nodeRadius);
                // Check if the point is obstructed or not
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unbuildableMask));
                NodeGrid[x, y] = new Node(walkable, worldPoint, x, y);
                if (walkable)
                {
                    // TODO Calculate where what building size can go
                    // TODO Migrate this to UI element
                    Instantiate(GameHandler.BuildableFlag, worldPoint, rotation);
                }

            }
        }
    }

    /// <summary>
    /// Get the neighbours around a node in a 3 x 3 space
    /// </summary>
    /// <param name="node">The node to get the neighbours from</param>
    /// <returns>List of the neighbouring nodes</returns>
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

                int checkX = node.GridX + x;
                int checkY = node.GridY + y;

                if (checkX >= 0 && checkX < GridSizeX && checkY >= 0 && checkY < GridSizeY)
                {
                    neighbours.Add(NodeGrid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    /// <summary>
    /// Get the node corresponding to a world point
    /// </summary>
    /// <param name="worldPos">World pos to get the node for</param>
    /// <returns>Node at the world position</returns>
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