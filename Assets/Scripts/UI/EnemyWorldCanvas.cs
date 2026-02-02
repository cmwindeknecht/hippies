using UnityEngine;
using UnityEngine.UI;

public class EnemyWorldCanvas : MonoBehaviour
{
    [SerializeField] private GameObject _HealthBar;
    [SerializeField] private Image _HealthBarBackground;
    [SerializeField] private Image _HealthBarOutline;
    [SerializeField] private Image _HealthBarFill;

    private Enemy _Enemy;
    private Vector3 _Offset;
    private const float _OffsetOffset = .3f; // How much to offset the offset by so it sits just above the enemy
    private const float _WidthAdjustment = 200f;

    void Start()
    {
        SpriteRenderer enemySprite = _Enemy.GetComponent<SpriteRenderer>();

        // Place at the enemy top + a small offset
        float enemyHeight = enemySprite.bounds.size.y;
        _Offset = new Vector3(0, (enemyHeight / 2f) + _OffsetOffset, 0); 

        // Make it the width of the enemy
        float enemyWidth = enemySprite.bounds.size.x;
        UpdateWidth(_HealthBarFill, enemyWidth);
        UpdateWidth(_HealthBarOutline, enemyWidth);
        UpdateWidth(_HealthBarBackground, enemyWidth);
    }

    private void UpdateWidth(Image component, float enemyWidth)
    {
        RectTransform rectTransform = component.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(enemyWidth * _WidthAdjustment, rectTransform.sizeDelta.y);
    }

    void LateUpdate()
    {
        if (_Enemy == null) return;
        transform.SetPositionAndRotation(_Enemy.transform.position + _Offset, Quaternion.identity);
    }

    public void RegisterEnemy(Enemy enemy)
    {
        _Enemy = enemy;
        _Enemy.OnHealthChanged += _Enemy_OnHealthChanged;
        _Enemy.TakeDamage(0); // TODO hacky way to have a current health bar
    }

    private void _Enemy_OnHealthChanged(object sender, Enemy.DyanmicStatChangeEvent e)
    {
        UpdateHealth(e.Current, e.Max);
    }

    void UpdateHealth(int currentHealth, int maxHealth)
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
        gameObject.SetActive(currentHealth != maxHealth);
        _HealthBarFill.fillAmount = (float)currentHealth / maxHealth;
    }
}
