using UnityEngine;

public class PlayerVisualChild : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _SpriteRenderer;

    private PlayerController _Controller;

    public void Setup(PlayerController controller)
    {
        _Controller = controller;
    }

    private void Update()
    {
        _SpriteRenderer.flipX = _Controller.FacingDirection.x < 0;
    }
}
