using UnityEngine;

public class PauseMenuCharacter : MonoBehaviour
{
    [SerializeField] private PauseMenuCharacterStatDisplay _Strength;
    [SerializeField] private PauseMenuCharacterStatDisplay _Agility;
    [SerializeField] private PauseMenuCharacterStatDisplay _Intelligence;
    [SerializeField] private PauseMenuCharacterStatDisplay _Vitality;
    [SerializeField] private PauseMenuCharacterStatDisplay _Stamina;
    [SerializeField] private PauseMenuCharacterStatDisplay _Luck;

    [SerializeField] private PauseMenuCharacterStatDisplay _WeaponOne;
    [SerializeField] private PauseMenuCharacterStatDisplay _WeaponTwo;

    [SerializeField] private PauseMenuCharacterStatDisplay _BluntDefense;
    [SerializeField] private PauseMenuCharacterStatDisplay _PiercingDefense;
    [SerializeField] private PauseMenuCharacterStatDisplay _ExplosiveDefense;
    [SerializeField] private PauseMenuCharacterStatDisplay _FireDefense;
    [SerializeField] private PauseMenuCharacterStatDisplay _IceDefense;
    [SerializeField] private PauseMenuCharacterStatDisplay _LightningDefense;
    [SerializeField] private PauseMenuCharacterStatDisplay _EarthDefense;
    [SerializeField] private PauseMenuCharacterStatDisplay _VoidDefense;

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
