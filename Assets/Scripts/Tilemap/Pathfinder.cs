using System;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS8632
public static class Pathfinder
{
    public enum CardinalDirection
    {
        N, E, S, W
    }

    public class PathfinderNode
    {
        public Vector3 nodePosition;
        public Vector3 nodeCenterPosition; // Cell center of this node
        public Vector3Int node3Int; // 3Int version of the node position (not center)
        public Vector2Int node2Int; // 2Int version of the node position (not center)
        public int fromStartCost; // How far have we walked from the starting point to reach this cell
        public int toGoalEstimatedCost; // Distance to goal as the crow flies
        public int totalEstimatedCost => fromStartCost + toGoalEstimatedCost; // Current estimate based on fromStartCost + toGoalEstimate
        public PathfinderNode? previousNode; // The last node traveled
    }

    private static Dictionary<Vector2Int, PathfinderNode> _DiscoveredNodes;
    private static HashSet<Vector2Int> _RuledOutNodes;
    private static List<Vector3> _FinalPath;

    public static List<Vector3> GetPath(Vector3 positionStart, Vector3 positionEnd)
    {
        Vector3Int end3Int = GridTilemapManager.Instance.GetTileFromPosition(positionEnd, TilemapType.Walkable);
        Vector3 endCellCenter = GridTilemapManager.Instance.GetTileCenter(end3Int, TilemapType.Walkable);

        _DiscoveredNodes = new();
        _FinalPath = new();
        _RuledOutNodes = new();

        PathfinderNode startNode = CreateNode(positionStart, positionEnd);
        _DiscoveredNodes.Add(startNode.node2Int, startNode);

        while (_DiscoveredNodes.Count > 0)
        {
            int lowestCost = int.MaxValue;
            PathfinderNode bestNode = null;
            foreach (PathfinderNode node in _DiscoveredNodes.Values)
            {
                if (lowestCost > node.totalEstimatedCost)
                {
                    lowestCost = node.totalEstimatedCost;
                    bestNode = node;
                }
            }

            if (bestNode == null)
            {
                throw new System.Exception("No path to player");
            }

            // Ones the best node is discovered, it should be ruled out so we don't retrace our steps
            _DiscoveredNodes.Remove(bestNode.node2Int);
            _RuledOutNodes.Add(bestNode.node2Int);

            PathfinderNode playerNode = null;
            if (Utilities.IsSameVectorPosition(bestNode.nodeCenterPosition, endCellCenter))
            {
                playerNode = bestNode;
            }
            else
            {
                // Search all the neighbors of the current best node
                foreach (CardinalDirection cardinalDirection in Enum.GetValues(typeof(CardinalDirection)))
                {
                    Vector2Int toCheck = GetTileByDirection(bestNode.node2Int, cardinalDirection);

                    // Skip ones we already ruled out
                    if (_RuledOutNodes.Contains(toCheck)) continue;

                    PathfinderNode neighborNode = CreateNode(new Vector3(toCheck.x, toCheck.y, 0), endCellCenter, bestNode);

                    // We're done, found the player 
                    if (Utilities.IsSameVectorPosition(neighborNode.nodeCenterPosition, endCellCenter))
                    {
                        playerNode = neighborNode;
                        break;
                    }
                    // Check if the neighbor can be walked
                    else if (GridTilemapManager.Instance.HasTile(neighborNode.node2Int, TilemapType.Walkable))
                    {
                        // Update the path if it is better
                        if (_DiscoveredNodes.ContainsKey(neighborNode.node2Int))
                        {
                            PathfinderNode existingNode = _DiscoveredNodes[neighborNode.node2Int];
                            int newFromStartCost = bestNode.fromStartCost + 1;
                            if (newFromStartCost < existingNode.fromStartCost)
                            {
                                existingNode.fromStartCost = newFromStartCost;
                                existingNode.previousNode = bestNode;
                            }
                        }
                        // Add it if its not already seen
                        else
                        {
                            _DiscoveredNodes[neighborNode.node2Int] = neighborNode;
                        }
                    }
                    // Rule it out if it can't be walked (and its not the player)
                    else
                    {
                        _RuledOutNodes.Add(neighborNode.node2Int);
                    }
                }
            }

            PathfinderNode nextNode = playerNode;
            while (nextNode != null)
            {
                _FinalPath.Insert(0, nextNode.nodeCenterPosition);
                nextNode = nextNode.previousNode;
            }
            if (_FinalPath.Count > 0)
            {
                break;
            }
        }

        return _FinalPath;
    }

    private static Vector2Int GetTileByDirection(Vector2Int position, CardinalDirection cardinalDirection)
    {
        return cardinalDirection switch
        {
            CardinalDirection.N => new Vector2Int(position.x, position.y + 1),
            CardinalDirection.E => new Vector2Int(position.x + 1, position.y),
            CardinalDirection.S => new Vector2Int(position.x, position.y - 1),
            CardinalDirection.W => new Vector2Int(position.x - 1, position.y),
            _ => throw new System.NotImplementedException()
        };
    }

    private static PathfinderNode CreateNode(Vector3 position, Vector3 endPosition, PathfinderNode previousNode = null)
    {
        Vector3Int position3Int = GridTilemapManager.Instance.GetTileFromPosition(position, TilemapType.Walkable);
        Vector3Int endPosition3Int = GridTilemapManager.Instance.GetTileFromPosition(endPosition, TilemapType.Walkable);

        Vector3 positionCellCenter = GridTilemapManager.Instance.GetTileCenter(position3Int, TilemapType.Walkable);
        Vector3 endPositionCellCenter = GridTilemapManager.Instance.GetTileCenter(endPosition3Int, TilemapType.Walkable);

        int fromStartCost = previousNode != null ? previousNode.fromStartCost + 1 : 0;
        int toGoalEstimatedCost = Utilities.RountToInt(Utilities.GetDistanceBetween(positionCellCenter, endPositionCellCenter));

        return new PathfinderNode
        {
            nodePosition = position,
            nodeCenterPosition = positionCellCenter,
            node2Int = new(position3Int.x, position3Int.y),
            node3Int = position3Int,
            fromStartCost = fromStartCost,
            toGoalEstimatedCost = toGoalEstimatedCost,
            previousNode = previousNode
        };
    }
}
