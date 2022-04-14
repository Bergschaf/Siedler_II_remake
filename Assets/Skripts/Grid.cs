using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
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
    public static float nodeRadius = 8;

    /// <summary>
    /// The diameter of a node
    /// </summary>
    private static float _nodeDiameter;

    /// <summary>
    /// The main node grid
    /// </summary>
    public static Node[,] NodeGrid;

    private void Start()
    {
        if (NodeGrid == null)
        {
            start();
        }
    }

    private static void start()
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
    static void CreateGrid()
    {
        NodeGrid = new Node[GridSizeX, GridSizeY];

        Vector3 worldBottomLeft = Vector3.zero; // Grid Game Object has to be on the bottom left of the terrain 
        Quaternion rotation = GameHandler.BuildableHouse3.transform.rotation;
        for (int x = 0; x < GridSizeX; x++)
        {
            for (int y = 0; y < GridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * _nodeDiameter + nodeRadius) +
                                     Vector3.forward * (y * _nodeDiameter + nodeRadius);
                // Check if the point is obstructed or not
                //bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unbuildableMask));
                bool walkable = true;
                NodeGrid[x, y] = new Node(walkable, worldPoint, x, y, "BuildableFlag",Instantiate(GameHandler.BuildableHouse3, worldPoint,rotation));

                // TODO Calculate where what building size can go
    
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
        if (NodeGrid == null)
        {
            start();
        }

        float percentX = (worldPos.x) / GridWorldSize.x;
        float percentY = (worldPos.z) / GridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((GridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((GridSizeY - 1) * percentY);

        return NodeGrid[x, y];
    }

    /// <summary>
    /// Returns the closest Flag to a world Position
    /// </summary>
    /// <param name="worldPos"></param>
    /// <returns></returns>
    public static FlagScript ClosestFlagToWorldPoint(Vector3 worldPos)
    {
        Node middleNode = NodeFromWorldPoint(worldPos);
        if(middleNode.Flag != null)
        {
            return middleNode.Flag;
        }

        int minX, minY, maxX, maxY;
        int maxSearchDistance = 20; // The Maximum Radius to search for a Flag
        for (int currentSearchDistance = 1; currentSearchDistance < maxSearchDistance; currentSearchDistance++)
        {
            minX = Mathf.Max(0, middleNode.GridX - currentSearchDistance);
            minY = Mathf.Max(0, middleNode.GridY - currentSearchDistance);
            maxX = Mathf.Min(GridSizeX - 1, middleNode.GridX + currentSearchDistance);
            maxY = Mathf.Min(GridSizeY - 1, middleNode.GridY + currentSearchDistance);

            for (int x = minX; x <= maxX; x++)
            {
                if (NodeGrid[x, maxY].Flag != null)
                {
                    return NodeGrid[x, maxY].Flag;
                }
                
                if (NodeGrid[x, minY].Flag != null)
                {
                    return NodeGrid[x, minY].Flag;
                }
                
            }
            
            for (int y = minY; y <= maxY; y++)
            {
                if (NodeGrid[maxX, y].Flag != null)
                {
                    return NodeGrid[maxX, y].Flag;
                }
                
                if (NodeGrid[minX, y].Flag != null)
                {
                    return NodeGrid[minX, y].Flag;
                }
                
            }
        }

        return null;
    }
}