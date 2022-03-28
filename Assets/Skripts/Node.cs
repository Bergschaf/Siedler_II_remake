using System;
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
    public int GCost, HCost;

    /// <summary>
    /// The position of the node in the grid
    /// </summary>
    public int GridX, GridY;

    /// <summary>
    /// The parent node, used for pathfinding
    /// </summary>
    public Node Parent;

    /// <summary>
    /// The Node Type (All types in NodeTypes.txt)
    /// </summary>
    public string Type;

    /// <summary>
    /// The Flag at the Node if a flag is at the Node
    /// </summary>
    public FlagScript Flag;

    /// <summary>
    /// The Road at the Node if there is one
    /// </summary>
    public Road Road;


    public Node(bool buildable, Vector3 worldPos, int gridX, int gridY, string type)
    {
        Buildable = buildable;
        WorldPosition = worldPos;
        GridX = gridX;
        GridY = gridY;
        Type = type;
    }

    public void CalculateBuildableType()
    {
        //TODO Calculate Buildable type
        if(Type != "Flag")
        {
            Type = "BuildableFlag";
            Buildable = true;
        }
    }

    /// <summary>
    /// Value for pathfinding
    /// </summary>
    public int FCost => GCost + HCost;
}