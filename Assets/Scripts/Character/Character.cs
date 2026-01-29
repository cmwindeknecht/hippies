using UnityEngine;

public enum CharacterType
{
    Enemy,
    Player
}

public class Character : MonoBehaviour
{
    protected Rigidbody2D _Rigidbody2D;
    public Vector2 Position => _Rigidbody2D == null ? Vector3.zero : (Vector3)_Rigidbody2D.position;
    // TODO flesh this out --- currently just has this because I needed to track the position of the player/enemy in the MeleeHitBox
}
