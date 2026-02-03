using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerAttackController _AttackController;
    private Rigidbody2D _RigidBody;

    private InputAction _MoveInputAction;
    private InputAction _AttackInputAction;

    private Vector2 _MoveValue = Vector2.zero;
    public Vector2 MoveValue => _MoveValue;

    private Vector2 _AttackValue = Vector2.zero;
    public Vector2 AttackValue => _AttackValue;

    private Vector2 _FacingDirection = Vector2.zero;
    public Vector2 FacingDirection => _FacingDirection;

    private Vector2 _LinearVelocity = Vector2.zero;

    private bool _ShouldZeroOut = true; // Plan is to make this false when knockback/environment modifies velocity
    private const float _BaseSpeed = 6f;
    private Vector2 _Knockback;

    private const float _AttackFacingLockTimeMaximum = 1f;
    private float _AttackFacingLockTime = 0f;

    private void Awake()
    {
        _RigidBody = GetComponent<Rigidbody2D>();
        _AttackController = GetComponent<PlayerAttackController>();

        _MoveInputAction = InputSystem.actions.FindAction("Move");
        _AttackInputAction = InputSystem.actions.FindAction("Attack");
    }

    void Update()
    {
        _MoveValue = _MoveInputAction.ReadValue<Vector2>();

        // Get mouse position in world space
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        // Direction from player to mouse
        Vector2 attackDirection = (mouseWorldPos - transform.position).normalized;

        if (Input.GetMouseButton(0)) // Left click
        {
            _AttackValue = attackDirection;
        }
        else
        {
            _AttackValue = Vector2.zero;
        }

        UpdateFacingDirection();
    }

    private void UpdateFacingDirection()
    {
        if (_AttackValue != Vector2.zero)
        {
            _FacingDirection = _AttackValue;
            if (_AttackController.TryAttack(_AttackValue))
            {
                _AttackFacingLockTime = Time.time + _AttackFacingLockTimeMaximum;
            }
        }
        else if (Time.time >= _AttackFacingLockTime && _MoveValue != Vector2.zero)
        {
            _FacingDirection = _MoveValue;
        }
    }

    private void FixedUpdate()
    {
        if (_Knockback.magnitude > 0.1f)
        {
            _ShouldZeroOut = false;
            _LinearVelocity = _Knockback;
            _Knockback = Vector2.Lerp(_Knockback, Vector2.zero, 5f * Time.fixedDeltaTime);
        }
        else
        {
            _ShouldZeroOut = true;
        }

        GetInputVelocity();
        SetRotation();
        MovePlayer();

        // Not going to have a jump (I think --- maybe?) but leaving this here for sneak or whatever
        //if (_JumpInputAction.IsPressed())
        //{
        //    _RigidBody.AddForce(transform.up * GetMovementSpeed(), ForceMode2D.Impulse);
        //}
    }

    public void Knockback(Vector3 direction, float speed)
    {
        // If I want to have stun at some point, do this 
        //      rb.AddForce(direction * strength, ForceMode2D.Impulse);
        //      knockbackEndTime = Time.time + 0.3f; // 0.3 second knockback duration
        //      And in the fixed update --- if (Time.time < knockbackEndTime) return

        _Knockback = direction * speed;
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

            _LinearVelocity = _MoveValue * GetMovementSpeed();
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

    private void MovePlayer()
    {
        Debug.Log($"Moving: velocity={_LinearVelocity}, moveValue={_MoveValue}, knockback={_Knockback}");

        _RigidBody.linearVelocity = _LinearVelocity;
    }
}