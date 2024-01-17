using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pathfinding))]
public class PathManager : MonoBehaviour
{
    public static PathManager Instance { get; private set; }

    Queue<PathRequest> myPathRequests = new Queue<PathRequest>();
    PathRequest myCurrentPathRequest;

    Pathfinding myPathFinding;
    bool myIsProcessingPath = false;

    void Awake()
    {
        Instance = this; 
        myPathFinding = GetComponent<Pathfinding>();
    }

    public static void RequestPath(Vector3 aPathStart, Vector3 aPathEnd, Action<Vector3[], bool> aCallback)
    {
        PathRequest newRequest = new PathRequest(aPathStart, aPathEnd, aCallback);
        Instance.myPathRequests.Enqueue(newRequest);
        Instance.TryProcessNext();
    }

    void TryProcessNext()
    {
        if (!myIsProcessingPath && myPathRequests.Count > 0) 
        { 
            myCurrentPathRequest = myPathRequests.Dequeue();
            myIsProcessingPath = true;
            myPathFinding.StartFindingPath(myCurrentPathRequest.myPathStart, myCurrentPathRequest.myPathEnd);
        }
    }

    struct PathRequest
    {
        public Vector3 myPathStart;
        public Vector3 myPathEnd;
        public Action<Vector3[], bool> myCallback;

        public PathRequest(Vector3 aStart, Vector3 anEnd, Action<Vector3[], bool> aCallback)
        {
            myPathStart = aStart;
            myPathEnd = anEnd;
            myCallback = aCallback;
        }
    }
}
