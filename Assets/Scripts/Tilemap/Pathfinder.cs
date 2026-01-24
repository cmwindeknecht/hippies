using System.Collections.Generic;
using UnityEngine;

public static class Pathfinder
{
    public class PathfinderNode
    {
        public Vector2Int position; // Position of this node
        public int fromStartCost; // How far have we walked from the starting point to reach this cell
        public int toGoalEstimatedCost; // Distance to goal as the crow flies
        public int totalEstimatedCost; // Current estimate based on fromStartCost + toGoalEstimate
        public PathfinderNode previousNode; // The last node traveled
    }

    public static HashSet<PathfinderNode> DiscoveredNodes;
    public static HashSet<PathfinderNode> RuledOutNodes;

    public static void GetPath(Vector3Int enemyPosition, Vector3Int playerPosition)
    {
        DiscoveredNodes = new();
        RuledOutNodes = new();

        DiscoveredNodes.Add(CreateNode(enemyPosition, playerPosition));


    }

    private static PathfinderNode CreateNode(Vector3Int currentPosition, Vector3Int playerPosition, PathfinderNode previousNode = null)
    {


        return new PathfinderNode
        {
            position = new Vector2Int(currentPosition.x, currentPosition.y),
            fromStartCost = previousNode != null ? previousNode.fromStartCost : 0,
            toGoalEstimatedCost = Utilities.GetDistanceBetween(previousNode != null ? previousNode),
            totalEstimatedCost = 0,
            previousNode = previousNode
        };
    }
}
