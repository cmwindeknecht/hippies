using System;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS8632
public static class Pathfinder
{
    public enum CardinalDirection
    {
        N, E, S, W
        //NE, SE, SW, NW
    }

    public class PathfinderNode
    {
        public Vector2Int position; // Position of this node
        public int fromStartCost; // How far have we walked from the starting point to reach this cell
        public int toGoalEstimatedCost; // Distance to goal as the crow flies
        public int totalEstimatedCost => fromStartCost + toGoalEstimatedCost; // Current estimate based on fromStartCost + toGoalEstimate
        public PathfinderNode? previousNode; // The last node traveled
    }

    private static Dictionary<Vector2Int, PathfinderNode> _DiscoveredNodes;
    private static HashSet<Vector2Int> _RuledOutNodes;
    private static List<Vector3> _FinalPath;

    // TODO all of this Vector3Int, Vector3, Vector2Int bullshit is annoying, fix it after I get everything working
    public static List<Vector3> GetPath(Vector3 enemyPosition3, Vector3 playerPosition3)
    {
        Vector3Int enemyPosition = GridTilemapManager.Instance.GetTileFromPosition(enemyPosition3, TilemapType.Walkable);
        Vector3Int playerPosition = GridTilemapManager.Instance.GetTileFromPosition(playerPosition3, TilemapType.Walkable);
        _DiscoveredNodes = new();
        _FinalPath = new();
        _RuledOutNodes = new(); 

        _DiscoveredNodes.Add(new Vector2Int(enemyPosition.x, enemyPosition.y), CreateNode(enemyPosition, playerPosition));

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
            _DiscoveredNodes.Remove(new Vector2Int(bestNode.position.x, bestNode.position.y));
            _RuledOutNodes.Add(bestNode.position);

            PathfinderNode playerNode = null;
            if (bestNode.position == new Vector2Int(playerPosition.x, playerPosition.y))
            {
                playerNode = bestNode;
            }
            else
            {
                // Search all the neighbors of the current best node
                foreach (CardinalDirection cardinalDirection in Enum.GetValues(typeof(CardinalDirection)))
                {
                    Vector2Int toCheck = GetTileByDirection(bestNode.position, cardinalDirection);
                    
                    // Skip ones we already ruled out
                    if (_RuledOutNodes.Contains(toCheck)) continue;

                    Vector3Int toCheck3 = new(toCheck.x, toCheck.y, 0);
                    PathfinderNode neighborNode = CreateNode(toCheck3, playerPosition, bestNode);

                    // We're done, found the player 
                    if (toCheck == new Vector2Int(playerPosition.x, playerPosition.y))
                    {
                        playerNode = neighborNode;
                        break;
                    }
                    // Check if the neighbor can be walked
                    else if (GridTilemapManager.Instance.HasTile(toCheck, TilemapType.Walkable))
                    {
                        // Update the path if it is better
                        if (_DiscoveredNodes.ContainsKey(toCheck))
                        {
                            PathfinderNode existingNode = _DiscoveredNodes[toCheck];
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
                            _DiscoveredNodes[toCheck] = neighborNode;
                        }
                    }
                    // Rule it out if it can't be walked (and its not the player)
                    else
                    {
                        _RuledOutNodes.Add(toCheck);
                    }
                }
            }

            PathfinderNode nextNode = playerNode;
            while (nextNode != null)
            {
                _FinalPath.Insert(0, GridTilemapManager.Instance.GetTileCenter(new Vector3Int(nextNode.position.x, nextNode.position.y, 0), TilemapType.Walkable));
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
            //CardinalDirection.NE => new Vector2Int(position.x + 1, position.y + 1),
            CardinalDirection.E => new Vector2Int(position.x + 1, position.y),
            //CardinalDirection.SE => new Vector2Int(position.x + 1, position.y - 1),
            CardinalDirection.S => new Vector2Int(position.x, position.y - 1),
            //CardinalDirection.SW => new Vector2Int(position.x - 1, position.y - 1),
            CardinalDirection.W => new Vector2Int(position.x - 1, position.y),
            //CardinalDirection.NW => new Vector2Int(position.x - 1, position.y + 1),
            _ => throw new System.NotImplementedException()
        };
    }

    private static PathfinderNode CreateNode(Vector3Int currentPosition, Vector3Int playerPosition, PathfinderNode previousNode = null)
    {
        int fromStartCost = previousNode != null ? previousNode.fromStartCost + 1 : 0;
        int toGoalEstimatedCost = Utilities.RountToInt(Utilities.GetDistanceBetween(currentPosition, playerPosition));

        return new PathfinderNode
        {
            position = new Vector2Int(currentPosition.x, currentPosition.y),
            fromStartCost = fromStartCost,
            toGoalEstimatedCost = toGoalEstimatedCost,
            previousNode = previousNode
        };
    }
}
