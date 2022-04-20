using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

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

    /// <summary>
    /// The Buildable GameObject, e.g. The little orange flag
    /// </summary>
    public GameObject BuildableIcon;


    public Node(bool buildable, Vector3 worldPos, int gridX, int gridY, string type, GameObject _buildableIcon)
    {
        Buildable = buildable;
        WorldPosition = worldPos;
        GridX = gridX;
        GridY = gridY;
        Type = type;
        BuildableIcon = _buildableIcon;
        BuildableIcon.SetActive(buildable);
    }

    public void CalculateBuildableTypeAround()
    {
        for (int i = GridX - 3; i < GridX + 3; i++)
        {
            for (int j = GridY - 3; j < GridY + 3; j++)
            {
                if (i < Grid.GridSizeX && j < Grid.GridSizeY && i >= 0 && j >= 0)
                {
                    Grid.NodeGrid[i, j].CalculateBuildableType();
                }
            }
        }
    }

    /// <summary>
    /// Determines what building size you can build on this node
    /// </summary>
    public void CalculateBuildableType()
    {
        if (Physics.CheckSphere(WorldPosition, Grid.nodeRadius, Grid.UnbuildableMask))
        {
            Type = "Obstructed";
            Buildable = false;
            BuildableIcon.SetActive(false);
            return;
        }

        if (Type != "Flag" && Type != "Building" && Type != "Road")
        {
            BuildableIcon.SetActive(true);
            // TODO Check Terrain height and angle here
            if (GridY == 0)
            {
                Type = "Buildable0";
                ChangeBuildableIcon(0);
                return;
            }

            if (!Grid.NodeGrid[GridX, GridY - 1].Buildable && Grid.NodeGrid[GridX, GridY - 1].Type != "Flag")
            {
                Type = "Buildable0";
                ChangeBuildableIcon(0);
                return;
            }

            int dist = Int32.MaxValue;
            for (int i = GridX - 3; i < GridX + 3; i++)
            {
                for (int j = GridY; j < GridY + 3; j++)
                {
                    if (i >= 0 && j >= 0 && i < Grid.GridSizeX && j < Grid.GridSizeY)
                    {
                        if (!Grid.NodeGrid[i, j].Buildable)
                        {
                            if (RoadPathfinding.GetDistance(this, Grid.NodeGrid[i, j]) < dist)
                            {
                                dist = RoadPathfinding.GetDistance(this, Grid.NodeGrid[i, j]);
                            }
                        }
                    }
                }
            }

            Buildable = true;
            if (dist < 10)
            {
                Type = "Buildable0";
                ChangeBuildableIcon(0);
            }
            else if (dist < 20)
            {
                Type = "Buildable1";
                ChangeBuildableIcon(1);
            }
            else if (dist < 30)
            {
                Type = "Buildable2";
                ChangeBuildableIcon(2);
                
            }
            else
            {
                Type = "Buildable3";
                ChangeBuildableIcon(3);
            }
        }
        else if (Type == "Road")
        {
            ChangeBuildableIcon(0);
            Buildable = true;
        }
        else
        {
            BuildableIcon.SetActive(false);
            Buildable = false;
        }
    }


    private void ChangeBuildableIcon(int buildableID)
    {
        Object.Destroy(BuildableIcon);
        if (buildableID == 0)
        {
            BuildableIcon = Object.Instantiate(GameHandler.BuildableFlag, WorldPosition, Quaternion.identity);
        }
        else if (buildableID == 1)
        {
            BuildableIcon = Object.Instantiate(GameHandler.BuildableHouse1, WorldPosition, Quaternion.identity);
        }
        else if (buildableID == 2)
        {
            BuildableIcon = Object.Instantiate(GameHandler.BuildableHouse2, WorldPosition, Quaternion.identity);
        }
        else if (buildableID == 3)
        {
            BuildableIcon = Object.Instantiate(GameHandler.BuildableHouse3, WorldPosition,
                GameHandler.BuildableHouse3.transform.rotation);
        }
    }

    /// <summary>
    /// Value for pathfinding
    /// </summary>
    public int FCost => GCost + HCost;
}