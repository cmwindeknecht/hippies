using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private PlayerController _Controller;
    private Animator _Animator;

    public void Setup(PlayerController controller)
    {
        _Controller = controller;
        _Animator = GetComponent<Animator>();
    }

    public void Update()
    {
        if (_Animator == null) throw new System.Exception("No Animator Component on PlayerVisual!");
        if (_Controller == null) return;

        Debug.Log($"Setting MoveValue: {_Controller.MoveValue.x}, MoveY: {_Controller.MoveValue.y}");
        Debug.Log($"Setting FacingValue: {_Controller.FacingDirection.x}, MoveY: {_Controller.FacingDirection.y}");

        _Animator.SetBool("IsWalking", _Controller.MoveValue != Vector2.zero);
        _Animator.SetFloat("MoveX", _Controller.FacingDirection.x);
        _Animator.SetFloat("MoveY", _Controller.FacingDirection.y);
    }

    void LateUpdate()
    {
        // Get parent's physics position
        Vector3 physicsPos = transform.parent.position;

        // Snap to pixels for rendering
        float pixelsPerUnit = 100f;
        physicsPos.x = Mathf.Round(physicsPos.x * pixelsPerUnit) / pixelsPerUnit;
        physicsPos.y = Mathf.Round(physicsPos.y * pixelsPerUnit) / pixelsPerUnit;

        // Apply snapped position to visual
        transform.position = physicsPos;
    }
}
