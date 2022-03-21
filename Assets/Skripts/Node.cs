using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool walkable;
    public int type;
    public Vector3 worldPosition;
    public int gCost;
    public int hCost;
    public int gridX;
    public int gridY;
    public Node parent;
    public Node(bool _walkable, Vector3 _world_Pos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _world_Pos;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int fCost => gCost + hCost;
}