using UnityEngine;

public class GameSfxListener : MonoBehaviour
{
    [SerializeField] AudioClip pickup, damage, win, lose;

    void OnEnable()
    {
        GameEvents.OrbCollected += OnCollected;
        GameEvents.PlayerDamaged += OnDamaged;
        GameEvents.GameWon += OnWon;
        GameEvents.GameLost += OnLost;
    }

    void OnDisable()
    {
        GameEvents.OrbCollected -= OnCollected;
        GameEvents.PlayerDamaged -= OnDamaged;
        GameEvents.GameWon -= OnWon;
        GameEvents.GameLost -= OnLost;
    }

    void OnCollected(int score, int goal) { AudioManager.Sfx(pickup); }
    void OnDamaged(int current, int max) { AudioManager.Sfx(damage); }
    void OnWon() { AudioManager.Sfx(win); }
    void OnLost() { AudioManager.Sfx(lose); }
}