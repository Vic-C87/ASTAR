using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    [SerializeField] NodeGrid myNodeGrid;

    void Awake()
    {
        
    }

    public void StartFindingPath(Vector3 aStartingPosition, Vector3 aTargetPosition)
    {
        StartCoroutine(FindPath(aStartingPosition, aTargetPosition));
    }

    IEnumerator FindPath(Vector3 aStartPosition, Vector3 aTargetPosition)
    {
        Vector3[] waypoints = new Vector3[0];
        bool foundPath = false;

        Node startNode = myNodeGrid.NodeFromWorldPoint(aStartPosition);
        Node targetNode = myNodeGrid.NodeFromWorldPoint(aTargetPosition);

        if (startNode.Walkable && targetNode.Walkable) 
        {
            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            while (openSet.Count > 0) 
            {
                Node currentNode = openSet[0];

                for (int i = 1; i < openSet.Count; i++) 
                {
                    if (openSet[i].FCost < currentNode.FCost || openSet[i].FCost == currentNode.FCost)
                    {
                        if (openSet[i].HCost < currentNode.HCost) 
                        {
                            currentNode = openSet[i];
                        }
                    }
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if(currentNode == targetNode) 
                {
                    foundPath = true;
                    break;
                }

                foreach (Node neighbour in myNodeGrid.GetNeighbours(currentNode))
                {
                    if (!neighbour.Walkable || closedSet.Contains(neighbour)) continue;

                    int newCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbour);

                    if (newCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
                    {
                        neighbour.GCost = newCostToNeighbour;
                        neighbour.HCost = GetDistance(neighbour, targetNode);
                        neighbour.Parent = currentNode;

                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                    }
                }

            }

        }
        yield return null;
        if (foundPath) 
        {
            waypoints = RetracePath(startNode, targetNode);
        }

    }

    Vector3[] RetracePath(Node aStartNode, Node aTargetNode)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Node currentNode = aTargetNode;

        while (currentNode != aStartNode)
        {
            waypoints.Add(currentNode.WorldPosition);
            currentNode = currentNode.Parent;
        }
        waypoints.Reverse();

        return waypoints.ToArray();
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.GridX - nodeB.GridX);
        int dstY = Mathf.Abs(nodeA.GridY - nodeB.GridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
