using Cysharp.Threading.Tasks;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Player _Player;
    private Rigidbody2D _RigidBody;

    private Vector3 _SpawnPosition = Vector3.zero;
    private bool _ShouldChasePlayer = false;
    
    // TODO EnemySO shit
    private float _Health = 5f;
    private float _BaseSpeed = 3f;
    private readonly float _MaxViewDistance = 10f; // I feel like some enemies should see longer than other
    private readonly float _OutOfSightChaseDistance = 5f; // Not sure if I should just do half the max view distance or this should be a const?

    // Pathfinding Shit
    private List<Vector3> _CurrentPath;
    private int _CurrentPathIndex;
    private bool _IsFollowingPath;

    private void Awake()
    {
        _RigidBody = GetComponent<Rigidbody2D>();
        _SpawnPosition = transform.position;
    }

    private void Update()
    {
        if (_Player == null)
        {
            return;
        }
        
        Vector3 playerDirection = (_Player.transform.position - transform.position).normalized;
        LayerMask playerLayerMask = LayerMask.GetMask("Player", "Collision");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, playerDirection, _MaxViewDistance, playerLayerMask);

        bool canSeePlayer = hit && hit.collider.CompareTag("Player");
        Color lineColor = canSeePlayer ? Color.green : Color.red;
        Debug.DrawRay(transform.position, playerDirection * _MaxViewDistance, lineColor);

        // If it sees the player / there are no collisions in between, start chasing
        if (canSeePlayer)
        {
            _ShouldChasePlayer = true;
        }
        // If the enemy loses sight, stop only if the player is too far away
        else if (_ShouldChasePlayer && Utilities.GetDistanceBetween(_Player.transform.position, transform.position) > _OutOfSightChaseDistance)
        {
            _ShouldChasePlayer = false;
        }

        // Either chase the player or return to the spawn position
        if (_ShouldChasePlayer)
        {
            // No collision --- go straight to player
            if (canSeePlayer)
            {
                _RigidBody.linearVelocity = (_Player.transform.position - transform.position).normalized * GetMovementSpeed();
            }
            // Get a path from the pathfinder
            else
            {
                if (!_IsFollowingPath || !Utilities.IsSameVectorPosition(_CurrentPath[^1], _Player.transform.position, 1f))
                {
                    _CurrentPath = Pathfinder.GetPath(transform.position, _Player.transform.position);
                    Debug.Log($"Getting new path !_IsFollowingPath {!_IsFollowingPath} || !Utilities.IsSameVectorPosition(_CurrentPath[_CurrentPath.Count - 1], _Player.transform.position, 1f) {!Utilities.IsSameVectorPosition(_CurrentPath[^1], _Player.transform.position, 1f)} _CurrentPath[^1] {_CurrentPath[^1]} _Player.transform.position {_Player.transform.position}");

                    if (_CurrentPath.Count <= 1)
                    {
                        _IsFollowingPath = false;
                        _RigidBody.linearVelocity = Vector2.zero;
                        throw new System.Exception("SETTING _IsFollowingPath FALSE --- SOMEHOW NO PATH TO PLAYER FOR ENEMY");
                    }

                    _CurrentPathIndex = 1;
                    _IsFollowingPath = true;

                    for (int i = 1; i < _CurrentPath.Count; i++)
                    {
                        Debug.DrawLine(_CurrentPath[i - 1], _CurrentPath[i], Color.blue);
                    }
                }

                // Safety clamp
                if (_CurrentPathIndex >= _CurrentPath.Count)
                {
                    Debug.Log($"SETTING _IsFollowingPath FALSE --- _CurrentPathIndex {_CurrentPathIndex} >= _CurrentPath.Count {_CurrentPath.Count}");
                    _IsFollowingPath = false;
                    _RigidBody.linearVelocity = Vector2.zero;
                    return;
                }

                Vector3 target = _CurrentPath[_CurrentPathIndex];
                Vector3 toTarget = target - transform.position;

                // Waypoint reached
                if (Utilities.IsSameVectorPosition(_CurrentPath[_CurrentPathIndex], transform.position))
                {
                    
                    _CurrentPathIndex++;
                    Debug.Log($"Advancing path index to {_CurrentPathIndex}");
                    _RigidBody.position = target;

                    // Stop if path complete
                    if (_CurrentPathIndex >= _CurrentPath.Count)
                    {
                        Debug.Log($"SETTING _IsFollowingPath FALSE (AGAIN?) --- _CurrentPathIndex {_CurrentPathIndex} >= _CurrentPath.Count {_CurrentPath.Count}");
                        _IsFollowingPath = false;
                        _RigidBody.linearVelocity = Vector2.zero;
                    }

                    return;
                }

                // Move toward current waypoint
                Vector2 desiredVelocity = toTarget.normalized * GetMovementSpeed();

                // If we're colliding, slide along the surface
                if (_RigidBody.IsTouchingLayers(LayerMask.GetMask("Collision")))
                {
                    ContactPoint2D[] contacts = new ContactPoint2D[4];
                    int count = _RigidBody.GetContacts(contacts);

                    for (int i = 0; i < count; i++)
                    {
                        Vector2 normal = contacts[i].normal;

                        // Remove component pushing into the surface
                        float intoSurface = Vector2.Dot(desiredVelocity, normal);
                        if (intoSurface < 0f)
                        {
                            desiredVelocity -= normal * intoSurface;
                        }
                    }
                }

                _RigidBody.linearVelocity = desiredVelocity;
            }
        }
        else if (!Utilities.IsSameVectorPosition(transform.position, _SpawnPosition))
        {
            _ShouldChasePlayer = false;
            _RigidBody.linearVelocity = (_SpawnPosition - transform.position).normalized * GetMovementSpeed();
        }
    }

    public void Setup(Player player)
    {
        _Player = player;
    }

    public void TakeDamage(float damage)
    {
        _Health -= damage;

        if (_Health < 0)
        {
            Destroy(gameObject);
        }
    }

    private float GetMovementSpeed()
    {
        // TODO use input to determine walk/sprint/sneak/etc
        return _BaseSpeed;
    }
}
