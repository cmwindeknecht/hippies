using UnityEngine;

public class PauseMenuCharacter : MonoBehaviour
{
    [SerializeField] private PauseMenuCharacterStatDisplay _Strength;
    [SerializeField] private PauseMenuCharacterStatDisplay _Agility;
    [SerializeField] private PauseMenuCharacterStatDisplay _Intelligence;
    [SerializeField] private PauseMenuCharacterStatDisplay _Vitality;
    [SerializeField] private PauseMenuCharacterStatDisplay _Stamina;
    [SerializeField] private PauseMenuCharacterStatDisplay _Luck;

    public void Setup(Player player)
    {
        _Strength.Setup(player);
        _Agility.Setup(player);
        _Intelligence.Setup(player);
        _Vitality.Setup(player);
        _Stamina.Setup(player);
        _Luck.Setup(player);
    }
}
