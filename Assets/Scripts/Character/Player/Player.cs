using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerController _Controller;

    private void Start()
    {
        _Controller = GetComponent<PlayerController>();

        GameManager.Instance.RegisterPlayer(this);
    }
}
