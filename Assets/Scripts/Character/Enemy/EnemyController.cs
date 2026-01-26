using Cysharp.Threading.Tasks;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Player _Player;
    private Rigidbody2D _RigidBody;
    private Vector3 _RigidBodyPosition => new(_RigidBody.position.x, _RigidBody.position.y, 0);

    private Vector3 _SpawnPosition = Vector3.zero;
    private bool _ShouldChasePlayer = false;
    
    // TODO EnemySO shit
    private float _BaseSpeed = 3f;
    private readonly float _MaxViewDistance = 10f; // I feel like some enemies should see longer than other
    private readonly float _OutOfSightChaseDistance = 5f; // Not sure if I should just do half the max view distance or this should be a const?

    // Pathfinding Shit
    private List<Vector3> _CurrentPath;
    private int _CurrentPathIndex;
    private bool _IsFollowingPath;

    public void Setup(Player player)
    {
        _Player = player;
    }

    private void Awake()
    {
        _RigidBody = GetComponent<Rigidbody2D>();
        _SpawnPosition = _RigidBody.position;
    }

    private void Update()
    {
        if (_Player == null)
        {
            return;
        }

        SimpleChasePlayer();
    }

    private void SimpleChasePlayer()
    {
        if (_Player == null)
        {
            return;
        }

        Vector3 playerDirection = (_Player.transform.position - _RigidBodyPosition).normalized;
        LayerMask playerLayerMask = LayerMask.GetMask("Player", "Collision");
        RaycastHit2D playerHit = Physics2D.Raycast(_RigidBodyPosition, playerDirection, _MaxViewDistance, playerLayerMask);
        bool canSeePlayer = playerHit && playerHit.collider.CompareTag("Player");

        // Scenario 1: Can see the player, chase them immediately
        if (canSeePlayer)
        {
            _ShouldChasePlayer = true;
            GoToPlayer();
        }
        else if (_ShouldChasePlayer)
        {
            // Scenario 3: Saw the player, no longer sees and is too far away to chase, pathfind back to spawn position
            if (Utilities.GetDistanceBetween(_Player.transform.position, _RigidBodyPosition) > _OutOfSightChaseDistance)
            {
                _ShouldChasePlayer = false;
                FollowPath(_SpawnPosition);
            }
            // Scenario 2: Saw the player, no longer sees but is close enough to chase
            else
            {
                FollowPath(_Player.transform.position);
            }
        }
        // Scenario 4: Can't see, too far away, return to spawn point if none of the above
        else if (!Utilities.IsSameVectorPosition(_RigidBodyPosition, _SpawnPosition, .1f))
        {
            FollowPath(_SpawnPosition);
        }
    }

    private void GoToPlayer()
    {
        _RigidBody.linearVelocity = (_Player.transform.position - _RigidBodyPosition).normalized * GetMovementSpeed();
    }

    private void FollowPath(Vector3 finalTarget)
    {
        if (!_IsFollowingPath || !Utilities.IsSameVectorPosition(_CurrentPath[^1], finalTarget, 1f))
        {
            _CurrentPath = Pathfinder.GetPath(_RigidBodyPosition, finalTarget);
            if (_CurrentPath.Count == 0)
            {
                StopMovement();
                throw new System.Exception($"SETTING _IsFollowingPath FALSE --- NO PATH --- Current Position {_RigidBodyPosition} Final Target Position {finalTarget}");
            }

            _CurrentPathIndex = 1;
            _IsFollowingPath = true;

            //for (int i = 1; i < _CurrentPath.Count; i++)
            //{
            //    Debug.DrawLine(_CurrentPath[i - 1], _CurrentPath[i], Color.blue);
            //}
        }

        if (_CurrentPathIndex >= _CurrentPath.Count)
        {
            StopMovement();
            return;
        }

        Vector3 target = _CurrentPath[_CurrentPathIndex];
        Vector3 toTarget = target - _RigidBodyPosition;

        if (Utilities.IsSameVectorPosition(_CurrentPath[_CurrentPathIndex], _RigidBodyPosition))
        {
            _CurrentPathIndex++;

            if (_CurrentPathIndex >= _CurrentPath.Count)
            {
                Debug.Log($"PATH COMPLETE --- _CurrentPathIndex {_CurrentPathIndex} >= _CurrentPath.Count {_CurrentPath.Count}");
                StopMovement();
            }

            return;
        }

        // Move toward current waypoint
        Vector2 desiredVelocity = toTarget.normalized * GetMovementSpeed();

        // If enemy is colliding, slide along the surface
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

    private void StopMovement()
    {
        _IsFollowingPath = false;
        _RigidBody.linearVelocity = Vector2.zero;
    }

    private float GetMovementSpeed()
    {
        // TODO use input to determine walk/sprint/sneak/etc
        if (_ShouldChasePlayer)
        {
            return _BaseSpeed;
        }
        else
        {
            return _BaseSpeed * .5f;
        }
    }
}
