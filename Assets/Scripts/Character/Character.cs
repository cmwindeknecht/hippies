using UnityEngine;

public enum CharacterType
{
    Enemy,
    Player
}

public class Character : MonoBehaviour
{
    private Rigidbody2D _Rigidbody2D;
    public Vector2 Position => _Rigidbody2D == null ? Vector3.zero : (Vector3)_Rigidbody2D.position;

    private void Awake()
    {
        _Rigidbody2D = GetComponent<Rigidbody2D>();
    }
    // TODO flesh this out --- currently just has this because I needed to track the position of the player/enemy in the MeleeHitBox
}
