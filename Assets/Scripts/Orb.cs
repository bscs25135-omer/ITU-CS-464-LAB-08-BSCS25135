using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Orb : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TMP_Text label;
    [SerializeField] float life = 2.5f;
    bool bad;

    public void Setup(bool isBad)
    {
        bad = isBad;
        image.color = bad ? new Color32(0x3C, 0xD6, 0xF2, 255) : new Color32(0xFF, 0x7A, 0x1A, 255);
        label.text = bad ? "X" : "+";          // not colour alone: symbol too
        Destroy(gameObject, life);
        StartCoroutine(Tween.Run(0.25f, Ease.OutBack,
            t => transform.localScale = Vector3.one * Mathf.LerpUnclamped(0.3f, 1f, t)));
    }

    public void OnTapped()
    {
        if (bad) GameController.Instance.Hit(); else GameController.Instance.Collect();
        Destroy(gameObject);
    }
}