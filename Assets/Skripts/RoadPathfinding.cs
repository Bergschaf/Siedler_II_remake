using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class RoadPathfinding : MonoBehaviour
{
    // Find Path from startPos to targetPos
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
                if (!neighbour.Buildable || closedSet.Contains(neighbour))
                {
                    continue;
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

    // Retrace the Calculated Path
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

    // Calculate the Distance between two Nodes
    static int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.GridX - nodeB.GridX);
        int dstY = Mathf.Abs(nodeA.GridY - nodeB.GridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}