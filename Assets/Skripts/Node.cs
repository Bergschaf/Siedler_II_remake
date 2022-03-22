using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Node in the main grid
/// </summary>
public class Node
{
    // TODO Implement the type here e.g. Flag buildable, big house buildable or road
    /// <summary>
    /// Are you able to build something on this node
    /// </summary>
    public bool Buildable;
    /// <summary>
    /// The world Position of the node
    /// </summary>
    public Vector3 WorldPosition;
    /// <summary>
    /// Values for pathfinding
    /// </summary>
    public int GCost,HCost;
    /// <summary>
    /// The position of the node in the grid
    /// </summary>
    public int GridX,GridY;
    /// <summary>
    /// The parent node, used for pathfinding
    /// </summary>
    public Node Parent;
    public Node(bool _buildable, Vector3 _world_Pos, int _gridX, int _gridY)
    {
        Buildable = _buildable;
        WorldPosition = _world_Pos;
        GridX = _gridX;
        GridY = _gridY;
    }

    /// <summary>
    /// Value for pathfinding
    /// </summary>
    public int FCost => GCost + HCost;
}