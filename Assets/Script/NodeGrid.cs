using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGrid : MonoBehaviour
{
    [SerializeField] LayerMask myUnwalkableMask;
    [SerializeField] Vector2 myGridWorldSize;
    [SerializeField] float myNodeRadius;

    float myNodeDiameter;
    int myGridSizeX;
    int myGridSizeY;

    Node[,] myGrid;

    void Awake()
    {
        myNodeDiameter = myNodeRadius * 2;
        myGridSizeX = Mathf.RoundToInt(myGridWorldSize.x/myNodeDiameter);
        myGridSizeY = Mathf.RoundToInt(myGridWorldSize.y / myNodeDiameter);
        CreateGrid();
    }

    void CreateGrid()
    {
        myGrid = new Node[myGridSizeX, myGridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * myGridWorldSize.x / 2 - Vector3.forward * myGridWorldSize.y / 2;

        for (int x = 0; x < myGridSizeX; x++) 
        {
            for (int y = 0; y < myGridSizeY; y++) 
            {
                Vector3 worldPoint = worldBottomLeft + (Vector3.right * (x * myNodeDiameter + myNodeRadius)) + (Vector3.forward * (y * myNodeDiameter + myNodeRadius));
                bool walkable = !(Physics.CheckSphere(worldPoint, myNodeRadius, myUnwalkableMask));
                myGrid[x,y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public List<Node> GetNeighbours(Node aNode)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++) 
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int checkX = aNode.GridX + x;
                int checkY = aNode.GridY + y;

                if (checkX >= 0 && checkX < myGridSizeX && checkY >= 0 && checkY < myGridSizeY)
                {
                    neighbours.Add(myGrid[checkX,checkY]);
                }
            }
        }

        return neighbours;
    }

    public Node NodeFromWorldPoint(Vector3 aWorldPoint)
    {
        float percentX = (aWorldPoint.x + myGridWorldSize.x / 2) / myGridWorldSize.x;
        float percentY = (aWorldPoint.z + myGridWorldSize.y / 2) / myGridWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((myGridSizeX-1) * percentX);
        int y = Mathf.RoundToInt((myGridSizeY-1) * percentY);

        Debug.Log(myGrid[x, y].WorldPosition);
        return myGrid[x,y];
    }



    //GIZMOS

    public List<Node> path;

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(myGridWorldSize.x, 1, myGridWorldSize.y));

        if (myGrid != null)
        {
            foreach (Node n in myGrid)
            {
                Gizmos.color = (n.Walkable) ? Color.white : Color.red;
                if (path != null)
                    if (path.Contains(n))
                        Gizmos.color = Color.black;
                Gizmos.DrawCube(n.WorldPosition, Vector3.one * (myNodeDiameter - .1f));
            }
        }
    }
}
