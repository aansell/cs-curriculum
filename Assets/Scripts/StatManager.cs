using UnityEngine;

public class StatManager : MonoBehaviour
{
    public static StatManager manager;

    [SerializeField] private int health = 3;
    [SerializeField] private int coins = 0;

    private void Awake()
    {
        if (manager != null && manager != this)
        {
            Destroy(gameObject);
        }
        else
        {
            manager = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        Debug.Log("Game Started! Health: " + health + " | Coins: " + coins);
    }

    public void ChangeHealth(int amount)
    {
        health += amount;
        Debug.Log("Health Changed! New Health: " + health);

        if (health <= 0)
        {
            Debug.Log("Player Died!");
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        // Halve coins rounded down
        coins = Mathf.FloorToInt(coins / 2f);
        
        // Reset health
        health = 3;

        // Reload Start scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Start");
    }

    public void ChangeCoins(int amount)
    {
        coins += amount;
        Debug.Log("Coins Changed! New Total: " + coins);
    }
}
