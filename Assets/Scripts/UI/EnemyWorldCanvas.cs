using UnityEngine;
using UnityEngine.UI;

public class EnemyWorldCanvas : MonoBehaviour
{
    [SerializeField] private GameObject _HealthBar;
    [SerializeField] private Image _HealthBarBackground;
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
        RectTransform rectTransformBackground = _HealthBarBackground.GetComponent<RectTransform>();
        RectTransform rectTransformFill = _HealthBarFill.GetComponent<RectTransform>();
        rectTransformBackground.sizeDelta = new Vector2(enemyWidth * _WidthAdjustment, rectTransformBackground.sizeDelta.y);
        rectTransformFill.sizeDelta = new Vector2(enemyWidth * _WidthAdjustment, rectTransformFill.sizeDelta.y);

        transform.SetParent(null); // Unparent from enemy so it doesn't rotate with the enemy
    }

    void LateUpdate()
    {
        transform.SetPositionAndRotation(_Enemy.transform.position + _Offset, Quaternion.identity);
    }

    public void RegisterEnemy(Enemy enemy)
    {
        _Enemy = enemy;
        _Enemy.OnHealthChanged += _Enemy_OnHealthChanged;
        _Enemy.TakeDamage(0); // TODO hacky way to have a current health bar
    }

    private void _Enemy_OnHealthChanged(object sender, Enemy.HealthChangedEventArgs e)
    {
        UpdateHealth(e.CurrentHealth, e.MaxHealth);
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
