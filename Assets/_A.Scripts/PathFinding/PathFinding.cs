using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    public static PathFinding Instance { get; private set; }
    public bool createDebug;

    [SerializeField] private Transform PathFindingDebugObject;
    [Tooltip("Height from ground to check for obstacles above the path")]
    [SerializeField] private float rayCastOffsetDistance = 5f;
    [SerializeField] internal LayerMask obstacleLayerMask;
    [SerializeField] internal LayerMask floorGridLayer;

    private GridSystem<PathNode> gridSystem;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;
    }

    public void SetUp(int width, int length, float cellSize)
    {
        gridSystem = new GridSystem<PathNode>(width, length, cellSize, (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));

        if (createDebug)
            gridSystem.CreateDebugObjects(PathFindingDebugObject);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < length; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);

                //if true the position has an obstacle
                if (Physics.Raycast(worldPosition + Vector3.down * rayCastOffsetDistance, Vector3.up, rayCastOffsetDistance * 2, obstacleLayerMask))
                    GetNode(x, z).SetIsWalkable(false);

            }
        }
    }

    public bool IsWalkableGridPosition(GridPosition gridPosition)
    {
        return gridSystem.GetGridObject(gridPosition).IsWalkable();
    }

    public void SetWalkableGridPosition(GridPosition gridPosition, bool isWalkable)
    {
        gridSystem.GetGridObject(gridPosition).SetIsWalkable(isWalkable);
    }

    public bool HasPath(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        return FindPath(startGridPosition, endGridPosition, out int pathLength) != null;
    }

    public int GetPathLength(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        FindPath(startGridPosition, endGridPosition, out int pathLength);
        return pathLength;
    }

    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition, out int pathLength)
    {
        //list to go through and list of blocked nodes
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        //copy path data and set first node up for search
        PathNode startNode = gridSystem.GetGridObject(startGridPosition);
        PathNode endNode = gridSystem.GetGridObject(endGridPosition);
        openList.Add(startNode);

        //go through grid
        for (int x = 0; x < gridSystem.GetWidth(); x++)
        {
            for (int z = 0; z < gridSystem.GetLength(); z++)
            {
                //set up nodes per grid
                GridPosition gridPosition = new GridPosition(x, z);
                PathNode pathNode = gridSystem.GetGridObject(gridPosition);

                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetCameFromPathNode();
            }
        }

        //
        startNode.SetGCost(0);
        //
        startNode.SetHCost(CalculateDistance(startGridPosition, endGridPosition));
        //
        startNode.CalculateFCost();

        //cicle while we have nodes on the list (finding best path)
        while (openList.Count > 0)
        {
            //get current node
            PathNode currentNode = GetLowestFCostPathNode(openList);

            //if best node is end node, calculate path
            if (currentNode == endNode)
            {
                // Reached final Node
                pathLength = endNode.GetFCost();
                return CalculatePath(endNode);
            }

            //if not, mark node as tested as not endNode
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            //checks all neighbours on current node
            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                //if node already closed then skip
                if (closedList.Contains(neighbourNode))
                    continue;

                if (!neighbourNode.IsWalkable())
                {
                    closedList.Add(neighbourNode);
                    continue;
                }

                //(not closed node) Move cost from current node to neighbour node
                int tentativeGCost = currentNode.GetGCost() + CalculateDistance(currentNode.GetGridPosition(), neighbourNode.GetGridPosition());

                //if cost is lower than neighbour, update neighbour of cheaper route
                if (tentativeGCost < neighbourNode.GetGCost())
                {
                    //update data
                    neighbourNode.SetCameFromPathNode(currentNode);
                    neighbourNode.SetGCost(tentativeGCost);
                    neighbourNode.SetHCost(CalculateDistance(neighbourNode.GetGridPosition(), endGridPosition));
                    neighbourNode.CalculateFCost();

                    //if neighbour not on the list, add so we can go though it
                    if (!openList.Contains(neighbourNode))
                        openList.Add(neighbourNode);
                }
            }
        }
        // No path found
        pathLength = 0;
        return null;
    }

    public int CalculateDistance(GridPosition gridPositionA, GridPosition gridPositionB)
    {
        GridPosition gridPositionDistance = gridPositionA - gridPositionB;

        int distance = Mathf.Abs(gridPositionDistance._x) + Mathf.Abs(gridPositionDistance._z);

        int xDistance = Mathf.Abs(gridPositionDistance._x);
        int zDistance = Mathf.Abs(gridPositionDistance._z);
        int remaining = Mathf.Abs(xDistance - zDistance);

        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostPathNode = pathNodeList[0];

        foreach (PathNode pathNode in pathNodeList)
            if (pathNode.GetFCost() < lowestFCostPathNode.GetFCost())
                lowestFCostPathNode = pathNode;

        return lowestFCostPathNode;
    }

    private PathNode GetNode(int x, int z)
    {
        return gridSystem.GetGridObject(new GridPosition(x, z));
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        GridPosition gridPosition = currentNode.GetGridPosition();

        if (gridPosition._x - 1 >= 0)
        {
            //Left
            neighbourList.Add(GetNode(gridPosition._x - 1, gridPosition._z));

            if (gridPosition._z + 1 < gridSystem.GetLength())
            {
                //Left Up
                neighbourList.Add(GetNode(gridPosition._x - 1, gridPosition._z + 1));
            }

            if (gridPosition._z - 1 >= 0)
            {
                //Left Down
                neighbourList.Add(GetNode(gridPosition._x - 1, gridPosition._z - 1));
            }
        }

        if (gridPosition._x + 1 < gridSystem.GetWidth())
        {
            //Right
            neighbourList.Add(GetNode(gridPosition._x + 1, gridPosition._z));

            if (gridPosition._z + 1 < gridSystem.GetLength())
            {
                //Right Up
                neighbourList.Add(GetNode(gridPosition._x + 1, gridPosition._z + 1));
            }

            if (gridPosition._z - 1 >= 0)
            {
                //Right Down
                neighbourList.Add(GetNode(gridPosition._x + 1, gridPosition._z - 1));
            }
        }

        if (gridPosition._z + 1 < gridSystem.GetLength())
        {
            //Up
            neighbourList.Add(GetNode(gridPosition._x + 0, gridPosition._z + 1));
        }

        if (gridPosition._z - 1 >= 0)
        {
            //Down
            neighbourList.Add(GetNode(gridPosition._x + 0, gridPosition._z - 1));
        }

        return neighbourList;
    }

    private List<GridPosition> CalculatePath(PathNode endNode)
    {
        //create path node list and start and the end
        List<PathNode> pathNodeList = new List<PathNode> { endNode };
        PathNode currentNode = endNode;

        //does it have a node it came from?
        while (currentNode.GetCameFromPathNode() != null)
        {
            //add connected node to path list
            pathNodeList.Add(currentNode.GetCameFromPathNode());
            currentNode = currentNode.GetCameFromPathNode();
        }

        //started from end so flip
        pathNodeList.Reverse();

        //create list for created path
        List<GridPosition> gridPositionList = new List<GridPosition>();

        //fill path list with game data
        foreach (PathNode pathNode in pathNodeList)
            gridPositionList.Add(pathNode.GetGridPosition());

        return gridPositionList;
    }

}