using UnityEngine;
using UnityEngine.UI;

public class HitFlashListener : MonoBehaviour
{
    [SerializeField] Image flash;   // full-screen red image, alpha 0, Raycast Target off

    void OnEnable() { GameEvents.PlayerDamaged += Flash; }
    void OnDisable() { GameEvents.PlayerDamaged -= Flash; }

    void Flash(int current, int max)
    {
        StopAllCoroutines();
        StartCoroutine(Tween.Run(0.35f, Ease.OutCubic, t =>
        {
            Color c = flash.color;
            c.a = 0.45f * (1f - t);
            flash.color = c;
        }));
    }
}