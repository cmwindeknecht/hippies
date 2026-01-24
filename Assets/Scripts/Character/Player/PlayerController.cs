using Cysharp.Threading.Tasks;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.UI;

public class PlayerController : MonoBehaviour
{


    [SerializeField] private GameObject _TestBullet;
    private Rigidbody2D _RigidBody;

    private InputAction _MoveInputAction;
    //private InputAction _JumpInputAction;
    private InputAction _AttackInputAction;

    private Vector2 _MoveValue = Vector2.zero;
    private Vector2 _AttackValue = Vector2.zero;
    private Vector2 _FacingDirection = Vector2.zero;
    private Vector2 _LinearVelocity = Vector2.zero;

    private bool _ShouldZeroOut = true; // Plan is to make this false when knockback/environment modifies velocity
    private readonly float _BaseSpeed = 6f;

    private bool _CanFireSingleShot = true;

    private void Awake()
    {
        _RigidBody = GetComponent<Rigidbody2D>();
        _MoveInputAction = InputSystem.actions.FindAction("Move");
        _AttackInputAction = InputSystem.actions.FindAction("Attack");
    }

    private void Update()
    {
        _MoveValue = _MoveInputAction.ReadValue<Vector2>();
        _AttackValue = _AttackInputAction.ReadValue<Vector2>();

        if (_AttackValue == Vector2.zero)
        {
            _CanFireSingleShot = true;
        }

        if (_AttackValue != Vector2.zero)
        {
            _FacingDirection = _AttackValue;

            if (_CanFireSingleShot)
            {
                _CanFireSingleShot = false;
                Attack();
            }
        }
        else if (_MoveValue != Vector2.zero)
        {
            _FacingDirection = _MoveValue;
        }
    }

    private void FixedUpdate()
    {
        
        //Debug.Log($"_MoveValue {_MoveValue}");

        GetInputVelocity();
        _RigidBody.linearVelocity = _LinearVelocity;
        //Debug.Log($"Velocity {_RigidBody.linearVelocity}");

        SetRotation();

        // Not going to have a jump (I think --- maybe?) but leaving this here for sneak or whatever
        //if (_JumpInputAction.IsPressed())
        //{
        //    _RigidBody.AddForce(transform.up * GetMovementSpeed(), ForceMode2D.Impulse);
        //}
    }

    // TODO This shit should be in an attack controller or maybe on weapons something.  Need to do shit like fire rate of gun and stuff, shouldn't be on the controller.  Controller should just be movement
    //          An argument for it being on the weapon is
    //              1) the weapon will know its bullet type
    //              2) the project needs to fire from the weapon position
    //   Since I need the attack direction, just send an event (setup the weaponcontroller with the player and subscribe)
    private void Attack()
    {
        GameObject gameObject = Instantiate(_TestBullet, transform.position + new Vector3(_FacingDirection.x, _FacingDirection.y), Quaternion.identity);
        TestBullet projectile = gameObject.GetComponent<TestBullet>();
        projectile.Fly(_FacingDirection);
    }

    private void SetRotation()
    {
        if (_FacingDirection != Vector2.zero)
        {
            _RigidBody.SetRotation(GetRotationAngle());
            // This just spins the player, doesn't set it to a position, leaving it here, could be good if I want to blow shit up and have them spin crazy as they are flung across the area
            //_RigidBody.MoveRotation(_RigidBody.rotation + _RotationSpeed * Time.fixedDeltaTime);
        }
    }

    // Modifies the velocity of the RigidBody2D based on player input
    // - If no external forces (_ShouldZeroOut = true), then it stops the player from moving by setting it to zero when there is no input
    private void GetInputVelocity()
    {
        if (_ShouldZeroOut)
        {
            _LinearVelocity = Vector2.zero;
        }

        if (_MoveValue != Vector2.zero)
        {
            // Player Mass already in this calculation
            // TODO armor/inventory/etc relative to strength should modify player mass

            // This kind of sucks for player movement, have to be a math / physics nerd to balance mass / damping / force etc which sucks if I let a player increase their speed
            // That being said --- I might need it for knockback / environment shit (moving platforms, water current, etc if I do that) so I am leaving it here
            //_RigidBody.AddForce(moveValue * GetMovementSpeed(), ForceMode2D.Impulse);

            _LinearVelocity += _MoveValue * GetMovementSpeed();
            //Debug.Log($"Velocity INSIDE {_LinearVelocity}");
        }
    }

    private float GetMovementSpeed()
    {
        // TODO use input to determine walk/sprint/sneak/etc
        return _BaseSpeed;
    }

    private float GetRotationAngle()
    {
        float rotation = _RigidBody.rotation;
        if (_FacingDirection != Vector2.zero)
        {
            // Get angle in degrees from vector
            float angle = Mathf.Atan2(_FacingDirection.y, _FacingDirection.x) * Mathf.Rad2Deg;

            // Snap to nearest 45°
            rotation = Mathf.Round(angle / 45f) * 45f;
        }
        return rotation;
    }
}