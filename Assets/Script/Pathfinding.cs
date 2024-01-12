using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    [SerializeField] Transform mySeeker;
    [SerializeField] Transform myTarget;

    [SerializeField] NodeGrid myNodeGrid;

    void Awake()
    {
        
    }

    void Start()
    {
        
        FindPath(mySeeker.position, myTarget.position);
    }

    void FindPath(Vector3 aStartPosition, Vector3 aTargetPosition)
    {
        Node startNode = myNodeGrid.NodeFromWorldPoint(aStartPosition);
        Node targetNode = myNodeGrid.NodeFromWorldPoint(aTargetPosition);

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
                RetracePath(startNode, targetNode);
                Debug.Log("FoundPath!");
                return;
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

    void RetracePath(Node aStartNode, Node aTargetNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = aTargetNode;

        while (currentNode != aStartNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }
        path.Reverse();

        myNodeGrid.path = path;
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
