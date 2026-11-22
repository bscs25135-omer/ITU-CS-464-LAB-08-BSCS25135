using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudListener : MonoBehaviour
{
    [SerializeField] Image healthFill;
    [SerializeField] TMP_Text scoreText;

    void OnEnable()
    {
        GameEvents.GameStarted += OnStarted;
        GameEvents.PlayerDamaged += OnDamaged;
        GameEvents.OrbCollected += OnCollected;
    }

    void OnDisable()
    {
        GameEvents.GameStarted -= OnStarted;
        GameEvents.PlayerDamaged -= OnDamaged;
        GameEvents.OrbCollected -= OnCollected;
    }

    void OnStarted(int max, int goal) { healthFill.fillAmount = 1f; scoreText.text = "ORBS 0 / " + goal; }
    void OnDamaged(int current, int max) { healthFill.fillAmount = (float)current / max; }
    void OnCollected(int score, int goal) { scoreText.text = "ORBS " + score + " / " + goal; }
}