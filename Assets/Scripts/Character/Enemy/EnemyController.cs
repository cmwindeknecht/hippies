using Cysharp.Threading.Tasks;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Player _Player;
    private EnemySO _EnemySO;
    private EnemyAttackController _AttackController;
    private Rigidbody2D _RigidBody;
    private Vector3 _RigidBodyPosition => new(_RigidBody.position.x, _RigidBody.position.y, 0);

    private Vector3 _SpawnPosition = Vector3.zero;
    private bool _ShouldChasePlayer = false;
    
    // TODO EnemySO shit
    private float _BaseSpeed = 3f;
    private readonly float _MaxViewDistance = 10f; // I feel like some enemies should see longer than other
    private readonly float _OutOfSightChaseDistance = 5f; // Not sure if I should just do half the max view distance or this should be a const?
    private Vector2 _Knockback;
    private const float _KnockbackDecay = 5f;

    // Pathfinding Shit
    private List<Vector3> _CurrentPath;
    private int _CurrentPathIndex;
    private bool _IsFollowingPath;

    public void Setup(EnemySO enemySO)
    {
        _EnemySO = enemySO;
    }

    private void Awake()
    {
        _RigidBody = GetComponent<Rigidbody2D>();
        _AttackController = GetComponent<EnemyAttackController>();
        _SpawnPosition = _RigidBody.position;
    }

    private void Start()
    {
        if (GameManager.Instance.Player != null)
        {
            _Player = GameManager.Instance.Player;
        }
        else
        {
            GameManager.Instance.OnPlayerRegistered += Instance_OnPlayerRegistered; ;
        }
    }

    private void Instance_OnPlayerRegistered(object sender, GameManager.OnPlayerRegisteredEventArgs e)
    {
        _Player = e.player;
    }

    private void Update()
    {
        if (_Player == null)
        {
            return;
        }

        SimpleChasePlayer();
    }

    public void Knockback(Vector3 direction, float speed)
    {
        // If I want to have stun at some point, do this 
        //      rb.AddForce(direction * strength, ForceMode2D.Impulse);
        //      knockbackEndTime = Time.time + 0.3f; // 0.3 second knockback duration
        //      And in the fixed update --- if (Time.time < knockbackEndTime) return

        _Knockback = direction * speed;
    }

    private void SimpleChasePlayer()
    {
        if (_Player == null)
        {
            return;
        }

        Vector3 playerDirection = (_Player.transform.position - _RigidBodyPosition).normalized;
        LayerMask playerLayerMask = LayerMask.GetMask(Constants.PLAYER_LAYER, Constants.COLLISION_LAYER);
        RaycastHit2D playerHit = Physics2D.Raycast(_RigidBodyPosition, playerDirection, _MaxViewDistance, playerLayerMask);
        bool canSeePlayer = playerHit && (playerHit.collider.CompareTag(Constants.PLAYER_TAG) || playerHit.collider.CompareTag(Constants.SHIELD_TAG));

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
        if (_EnemySO.WeaponSO == null) throw new System.Exception("Enemy has no weapon SO!"); // even a weaponless enemy should have an unarmed SO

        if (_Knockback.magnitude < 0.1f)
        {
            _Knockback = Vector2.zero;
        }

        // If in range of the player, either attack or don't move
        Vector2 movement = Vector2.zero;
        bool shouldChasePlayer = true;

        // Only attack / rotate if there is no knockback
        if (_Knockback == Vector2.zero)
        {
            if (_EnemySO.WeaponSO is MeleeWeaponSO)
            {
                MeleeWeaponSO meleeWeaponSO = _EnemySO.WeaponSO as MeleeWeaponSO;
                // Check if the enemy is in range 
                if (Utilities.IsSameVectorPosition(_RigidBodyPosition + transform.right * meleeWeaponSO.Reach, _Player.transform.position))
                {
                    shouldChasePlayer = false;
                    _AttackController.TryAttack(transform.right);
                }
            }
            else if (_EnemySO.WeaponSO is RangedWeaponSO)
            {
                RangedWeaponSO rangedWeaponSO = _EnemySO.WeaponSO as RangedWeaponSO;
                Vector2 toPlayer = _Player.transform.position - _RigidBodyPosition;
                float distanceToPlayer = toPlayer.magnitude;

                // Player is within attack range
                if (distanceToPlayer <= Mathf.Max(_EnemySO.AttackRange, rangedWeaponSO.Range))
                {
                    shouldChasePlayer = false;
                    _AttackController.TryAttack(toPlayer.normalized); // Attack toward player
                }
            }

            Vector2 rotation = (_Player.transform.position - _RigidBodyPosition).normalized;
            float angle = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        // If knockback occurred, overwrite the possibly stalled movement due to proximity
        if (_Knockback.magnitude > 0.1f)
        {
            movement = _Knockback;
            _Knockback = Vector2.Lerp(_Knockback, Vector2.zero, _KnockbackDecay * Time.fixedDeltaTime);
        }
        // Keep chasing if no knockback / no proximity
        else if (shouldChasePlayer)
        {
            movement = (_Player.transform.position - _RigidBodyPosition).normalized * GetMovementSpeed();
        }

        _RigidBody.linearVelocity = movement;
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
                //Debug.Log($"PATH COMPLETE --- _CurrentPathIndex {_CurrentPathIndex} >= _CurrentPath.Count {_CurrentPath.Count}");
                StopMovement();
            }

            return;
        }

        // Move toward current waypoint
        Vector2 movement = toTarget.normalized * GetMovementSpeed();

        // If enemy is colliding, slide along the surface
        if (_RigidBody.IsTouchingLayers(LayerMask.GetMask(Constants.COLLISION_LAYER)))
        {
            ContactPoint2D[] contacts = new ContactPoint2D[4];
            int count = _RigidBody.GetContacts(contacts);

            for (int i = 0; i < count; i++)
            {
                Vector2 normal = contacts[i].normal;

                // Remove component pushing into the surface
                float intoSurface = Vector2.Dot(movement, normal);
                if (intoSurface < 0f)
                {
                    movement -= normal * intoSurface;
                }
            }
        }

        if (_Knockback.magnitude > 0.1f)
        {
            movement = _Knockback;
            _Knockback = Vector2.Lerp(_Knockback, Vector2.zero, _KnockbackDecay * Time.fixedDeltaTime);
        }

        if (movement != Vector2.zero)
        {
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        _RigidBody.linearVelocity = movement;
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
