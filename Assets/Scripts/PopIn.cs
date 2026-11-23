using UnityEngine;

public class PopIn : MonoBehaviour
{
    [SerializeField] float from = 0.7f;
    [SerializeField] float duration = 0.45f;

    void OnEnable() { Play(); }

    public void Play()
    {
        StopAllCoroutines();
        StartCoroutine(Tween.Run(duration, Ease.OutBack,
            t => transform.localScale = Vector3.one * Mathf.LerpUnclamped(from, 1f, t)));
    }
}