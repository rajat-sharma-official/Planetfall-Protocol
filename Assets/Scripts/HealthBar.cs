using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;

    private void Awake()
    {
        
        if (healthSlider == null)
        {
            healthSlider = GetComponent<Slider>();
        }
    }

    public void Initialize(float maxHealth, float currentHealth)
    {
        if (healthSlider == null) return;

        healthSlider.minValue = 0f;
        healthSlider.maxValue = maxHealth;
        healthSlider.value    = currentHealth;
    }

    public void SetHealth(float currentHealth)
    {
        if (healthSlider == null) return;
        healthSlider.value = currentHealth;
    }
}