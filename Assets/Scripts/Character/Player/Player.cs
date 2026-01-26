using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerController _Controller;
    private PlayerInventory _Inventory;

    private void Start()
    {
        _Controller = GetComponent<PlayerController>();
        _Inventory = GetComponent<PlayerInventory>();

        GameManager.Instance.RegisterPlayer(this);
    }
}
