using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _RigidBody;

    private InputAction _MoveInputAction;
    //private InputAction _JumpInputAction;

    private Vector2 _LinearVelocity = Vector2.zero;
    private bool _ShouldZeroOut = true; // Plan is to make this false when knockback/environment modifies velocity
    private readonly float _BaseSpeed = 6f;

    private void Awake()
    {
        _RigidBody = GetComponent<Rigidbody2D>();
        _MoveInputAction = InputSystem.actions.FindAction("Move");
    }

    private void FixedUpdate()
    {
        GetInputVelocity();
        _RigidBody.linearVelocity = _LinearVelocity;

        // Not going to have a jump (I think --- maybe?) but leaving this here for sneak or whatever
        //if (_JumpInputAction.IsPressed())
        //{
        //    _RigidBody.AddForce(transform.up * GetMovementSpeed(), ForceMode2D.Impulse);
        //}
    }

    // Modifies the velocity of the RigidBody2D based on player input
    // - If no external forces (_ShouldZeroOut = true), then it stops the player from moving by setting it to zero when there is no input
    private void GetInputVelocity()
    {
        if (_ShouldZeroOut)
        {
            _LinearVelocity = Vector2.zero;
        }
        
        Vector2 moveValue = _MoveInputAction.ReadValue<Vector2>();
        if (moveValue != Vector2.zero)
        {
            // Player Mass already in this calculation
            // TODO armor/inventory/etc relative to strength should modify player mass

            // This kind of sucks for player movement, have to be a math / physics nerd to balance mass / damping / force etc which sucks if I let a player increase their speed
            // That being said --- I might need it for knockback / environment shit (moving platforms, water current, etc if I do that) so I am leaving it here
            //_RigidBody.AddForce(moveValue * GetMovementSpeed(), ForceMode2D.Impulse);

            _LinearVelocity += moveValue * GetMovementSpeed();
            //Debug.Log($"Velocity {_RigidBody.linearVelocity}");
        }
    }

    private float GetMovementSpeed()
    {
        // TODO use input to determine walk/sprint/sneak/etc
        return _BaseSpeed;
    }
}
