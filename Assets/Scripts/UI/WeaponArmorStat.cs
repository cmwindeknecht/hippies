using TMPro;
using UnityEngine;

public class WeaponArmorStat : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _ValueText;
    [SerializeField] private TextMeshProUGUI _MinValueText;
    [SerializeField] private TextMeshProUGUI _MaxValueText;
    public float Value;
    public float MinValue;
    public float MaxValue;

    public void Setup(float value)
    {
        _ValueText.gameObject.SetActive(true);
        _MinValueText.gameObject.SetActive(false);
        _MaxValueText.gameObject.SetActive(false);

        Value = value;
        _ValueText.text = value.ToString();
    }

    public void Setup(float value, float maxValue)
    {
        _ValueText.gameObject.SetActive(false);
        _MinValueText.gameObject.SetActive(true);
        _MaxValueText.gameObject.SetActive(true);

        MinValue = value;
        MaxValue = maxValue;
        _MinValueText.text = value.ToString();
        _MaxValueText.text = maxValue.ToString();
    }

    public void Setup(Color color, string value)
    {
        _ValueText.gameObject.SetActive(true);
        _MinValueText.gameObject.SetActive(false);
        _MaxValueText.gameObject.SetActive(false);

        _ValueText.text = value;
        _ValueText.color = color;
    }

    public void Setup(Color color, string value, Color maxColor, string maxValue)
    {
        _ValueText.gameObject.SetActive(false);
        _MinValueText.gameObject.SetActive(true);
        _MaxValueText.gameObject.SetActive(true);

        _MinValueText.text = value;
        _MinValueText.color = color;
        _MaxValueText.text= maxValue;
        _MaxValueText.color = maxColor;
    }
}
