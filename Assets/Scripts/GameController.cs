using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    [SerializeField] int maxHealth = 5;
    [SerializeField] int goal = 10;
    int health, score;
    public bool Running { get; private set; }

    void Awake() { Instance = this; }

    void Start()
    {
        health = maxHealth;
        score = 0;
        Running = true;
        Time.timeScale = 1f;
        AudioManager.Pause(false);
        GameEvents.RaiseGameStarted(maxHealth, goal);
    }

    public void Collect()
    {
        if (!Running) return;
        score++;
        GameEvents.RaiseOrbCollected(score, goal);
        if (score >= goal) End(true);
    }

    public void Hit()
    {
        if (!Running) return;
        health = Mathf.Max(0, health - 1);
        GameEvents.RaisePlayerDamaged(health, maxHealth);
        if (health <= 0) End(false);
    }

    void End(bool won)
    {
        Running = false;
        if (won) GameEvents.RaiseGameWon(); else GameEvents.RaiseGameLost();
    }
}