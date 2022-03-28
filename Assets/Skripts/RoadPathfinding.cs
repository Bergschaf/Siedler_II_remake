using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Class for finding a path for a road
/// </summary>
public class RoadPathfinding : MonoBehaviour
{
    /// <summary>
    /// Find a path from the start to the target position, using the A* algorithm
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="targetPos"></param>
    /// <returns></returns>
    public static List<Node> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = Grid.NodeFromWorldPoint(startPos);
        Node targetNode = Grid.NodeFromWorldPoint(targetPos);
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node node = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < node.FCost || openSet[i].FCost == node.FCost)
                {
                    if (openSet[i].HCost < node.HCost)
                        node = openSet[i];
                }
            }

            openSet.Remove(node);
            closedSet.Add(node);

            if (node == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            foreach (Node neighbour in Grid.GetNeighbours(node))
            {
                if (closedSet.Contains(neighbour))
                {
                    continue;
                }

                if (neighbour.Type != "Flag" && neighbour != targetNode && !neighbour.Buildable)

                {
                    continue;
                }


                // If the connection between the two nodes is diagonal
                if (node.GridX != neighbour.GridX && node.GridY != neighbour.GridY &&
                    (node.Type == "Flag" || node.Type == "Road") ||
                    (neighbour.Type == "Flag" || neighbour.Type == "Road"))
                {
                    Node node1 = Grid.NodeGrid[node.GridX, neighbour.GridY];
                    Node node2 = Grid.NodeGrid[neighbour.GridX, node.GridY];

                    Road road1 = node1.Road;

                    if (road1 == null && node1.Type == "Flag" && node1.Flag.AttachedRoads != null)
                    {
                        foreach (var r in node1.Flag.AttachedRoads)
                        {
                            if (r.Item1.Nodes.Contains(node1))
                            {
                                road1 = r.Item1;
                            }
                        }
                    }

                    Road road2 = node2.Road;
                    if (road2 == null && node2.Type == "Flag" && node2.Flag.AttachedRoads != null)
                    {
                        foreach (var r in node2.Flag.AttachedRoads)
                        {
                            if (r.Item1.Nodes.Contains(node2))
                            {
                                road2 = r.Item1;
                            }
                        }
                    }

                    if (road1 != null && road2 != null)
                    {
                        // If there is a diagonal Road across the other road

                        if (road1.Nodes.Contains(node2) || road2.Nodes.Contains(node1))
                        {
                            continue;
                        }
                    }
                }


                int newCostToNeighbour = node.GCost + GetDistance(node, neighbour);
                if (newCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
                {
                    neighbour.GCost = newCostToNeighbour;
                    neighbour.HCost = GetDistance(neighbour, targetNode);
                    neighbour.Parent = node;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }

        return new List<Node>();
    }

    /// <summary>
    /// Retrace the calculating path
    /// </summary>
    /// <param name="startNode"></param>
    /// <param name="endNode"></param>
    /// <returns></returns>
    static List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }

        path.Add(currentNode);

        path.Reverse();

        return path;
    }

    /// <summary>
    /// Calculate the distance between two nodes
    /// </summary>
    /// <param name="nodeA"></param>
    /// <param name="nodeB"></param>
    /// <returns></returns>
    static int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.GridX - nodeB.GridX);
        int dstY = Mathf.Abs(nodeA.GridY - nodeB.GridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}