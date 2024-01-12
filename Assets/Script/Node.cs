using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public bool Walkable { get; set; }
    public Vector3 WorldPosition { get; set; }
    public int GridX { get; set; }
    public int GridY { get; set; }

    public int GCost { get; set; }
    public int HCost { get; set; }

    public int FCost 
    {
        get
        {
            return GCost + HCost;
        } 
    }

    public Node Parent { get; set; }

    public Node(bool aWalkable, Vector3 aWorldPosition, int aGridX, int aGridY)
    {
        Walkable = aWalkable;
        WorldPosition = aWorldPosition;
        GridX = aGridX;
        GridY = aGridY;
    }
}
