using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class PauseMenuCharacterComparableAbstract : MonoBehaviour
{
    [SerializeField] private Image _ItemIcon;
    [SerializeField] protected WeaponArmorStat _ItemWeightPrefab;
    [SerializeField] protected WeaponArmorStat _FirePrefab;
    [SerializeField] protected WeaponArmorStat _IcePrefab;
    [SerializeField] protected WeaponArmorStat _LightningPrefab;
    [SerializeField] protected WeaponArmorStat _EarthPrefab;
    [SerializeField] protected WeaponArmorStat _VoidPrefab;
    [SerializeField] protected WeaponArmorStat _PiercingPrefab;
    [SerializeField] protected WeaponArmorStat _BluntPrefab;
    [SerializeField] protected WeaponArmorStat _ExplosivePrefab;

    [SerializeField] private TextMeshProUGUI _ItemNameText;

    [SerializeField] private Button _CompareButton;
    [SerializeField] private Button _EquipButton;

    [SerializeField] private RectTransform _StatContainer;

    private void Awake()
    {
        _CompareButton.gameObject.SetActive(true);
        _EquipButton.gameObject.SetActive(false);

        _CompareButton.onClick.AddListener(() =>
        {
            _CompareButton.gameObject.SetActive(false);
            _EquipButton.gameObject.SetActive(true);
            // Send compare event
        });
        _EquipButton.onClick.AddListener(() =>
        {
            // Send equip event
        });
    }

    protected void Setup(ItemSO itemSO)
    {
        _ItemNameText.text = itemSO.Name;

        _ItemWeightPrefab.gameObject.SetActive(true);
        _ItemWeightPrefab.Setup(itemSO.Weight);
    }

    protected void SetupFire(float value, int? maxValue = null, bool isActive = true) => SetupElement(_FirePrefab, value, maxValue, isActive);

    protected void SetupIce(float value, int? maxValue = null, bool isActive = true) => SetupElement(_IcePrefab, value, maxValue, isActive);

    protected void SetupLightning(float value, int? maxValue = null, bool isActive = true) => SetupElement(_LightningPrefab, value, maxValue, isActive);

    protected void SetupEarth(float value, int? maxValue = null, bool isActive = true) => SetupElement(_EarthPrefab, value, maxValue, isActive);

    protected void SetupVoid(float value, int? maxValue = null, bool isActive = true) => SetupElement(_VoidPrefab, value, maxValue, isActive);

    protected void SetupPiercing(float value, int? maxValue = null, bool isActive = true) => SetupElement(_PiercingPrefab, value, maxValue, isActive);

    protected void SetupBlunt(float value, int? maxValue = null, bool isActive = true) => SetupElement(_BluntPrefab, value, maxValue, isActive);

    protected void SetupExplosive(float value, int? maxValue = null, bool isActive = true) => SetupElement(_ExplosivePrefab, value, maxValue, isActive);

    private void SetupElement(WeaponArmorStat prefab, float value, int? maxValue, bool isActive)
    {
        if (maxValue.HasValue)
        {
            prefab.Setup(value, maxValue.Value);
        }
        else
        {
            prefab.Setup(value);
        }
        prefab.gameObject.SetActive(isActive);
    }

    public void Reset()
    {
        _CompareButton.gameObject.SetActive(true);
        _EquipButton.gameObject.SetActive(false);
    }
}
