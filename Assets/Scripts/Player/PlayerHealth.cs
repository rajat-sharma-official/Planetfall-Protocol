using UnityEngine;
using UnityEngine.SceneManagement;   
using UnityEngine.InputSystem;      
                                    
public class PlayerHealth : MonoBehaviour, IDataPersistence
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    [Header("UI")]
    [SerializeField] private HealthBar healthBar;   // drag from HealthHUD in the scene

    private void Start()
    {
        // If we didn't load anything yet, start full
        if (currentHealth <= 0f)
        {
            currentHealth = maxHealth;
        }

        if (healthBar != null)
        {
            healthBar.Initialize(maxHealth, currentHealth);
        }
    }

    // ===== Public API for hazards / enemies =====
    public void TakeDamage(float amount)
    {
        if (amount <= 0f || currentHealth <= 0f) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (amount <= 0f || currentHealth <= 0f) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
    }

    private void Die()
    {
        Debug.Log("Player died. Loading most recent save...");

        
        // Stop hazard UI immediately
        HazardWarningUI.Instance?.ForceClearAll();

        // Reset all hazard zones (teleport may skip trigger exits)
        foreach (var hz in FindObjectsOfType<DamageZone>())
        {
            hz.ForceReset();
        }

        if (DataPersistenceManager.instance != null)
        {
            // LoadGame will call LoadData on PlayerController, PlayerHealth, etc.
            DataPersistenceManager.instance.LoadGame();
        }
        else
        {
            // Fallback if something is wrong with the save system
            Debug.LogWarning("No DataPersistenceManager found, reloading current scene instead.");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    // ===== IDataPersistence implementation =====
    public void LoadData(GameData data)
    {
        // Use whatever is in the save file
        currentHealth = Mathf.Clamp(data.playerHealth, 0f, maxHealth);

        if (healthBar != null)
        {
            healthBar.Initialize(maxHealth, currentHealth);
        }
    }

    public void SaveData(ref GameData data)
    {
        data.playerHealth = currentHealth;
    }

    // ===== Optional debug: Space to take damage, like you had =====
    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.backslashKey.wasPressedThisFrame)
        {
            TakeDamage(100f);
        }
    }
}
