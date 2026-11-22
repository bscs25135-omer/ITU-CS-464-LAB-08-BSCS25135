using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScreenId
{
    Title, Main, Mode, Legends, Shop, Settings,
    Match, Pause, Results, Credits, Loading
}

public class ScreenManager : MonoBehaviour
{
    [SerializeField] ScreenPanel[] panels;

    readonly Stack<ScreenPanel> stack = new Stack<ScreenPanel>();
    bool busy;
    const float Shift = 120f;

    public static bool StartAtMain;

    void Start()
    {
        foreach (var p in panels) SetVisible(p, false);

        ScreenPanel first = Find(ScreenId.Title);
        stack.Push(first);

        if (StartAtMain)
        {
            first = Find(ScreenId.Main);
            stack.Push(first);
            StartAtMain = false;
        }

        SetVisible(first, true);
        first.onShown.Invoke();
    }

    void Update()   // Escape = Android back button
    {
        if (stack.Peek().id == ScreenId.Loading) return;

        if (!Input.GetKeyDown(KeyCode.Escape) || busy) return;

        if (stack.Peek().id == ScreenId.Match)
            Go((int)ScreenId.Pause);
        else
            Back();
    }

    ScreenPanel Find(ScreenId id)
    {
        foreach (var p in panels)
            if (p.id == id) return p;

        return null;
    }

    public void Go(int id)          // forward: push a screen
    {
        if (busy) return;

        AudioManager.UI(UiSfx.Open);

        var from = stack.Peek();
        var to = Find((ScreenId)id);

        stack.Push(to);

        StartCoroutine(to.isOverlay
            ? OverlayIn(from, to)
            : Slide(from, to, 1f));
    }

    public void GoRoot(int id)      // jump: clear history (Menu, Rematch, Quit)
    {
        if (busy) return;

        var from = stack.Peek();
        var to = Find((ScreenId)id);

        stack.Clear();

        if (id == (int)ScreenId.Main)
            stack.Push(Find(ScreenId.Title));

        stack.Push(to);

        StartCoroutine(Slide(from, to, -1f));
    }

    public void Back()
    {
        if (busy || stack.Count < 2) return;

        AudioManager.UI(UiSfx.Close);

        var from = stack.Pop();
        var to = stack.Peek();

        StartCoroutine(from.isOverlay
            ? OverlayOut(from, to)
            : Slide(from, to, -1f));
    }

    static void SetVisible(ScreenPanel p, bool on)
    {
        p.group.alpha = on ? 1f : 0f;
        p.group.interactable = on;
        p.group.blocksRaycasts = on;

        p.content.anchoredPosition = Vector2.zero;

        if (p.card)
            p.card.localScale = Vector3.one;
    }

    // dir = 1 forward (new screen arrives from the right)
    // dir = -1 back (new screen arrives from the left)
    IEnumerator Slide(ScreenPanel from, ScreenPanel to, float dir)
    {
        busy = true;

        foreach (var p in panels)
            if (p != from && p != to)
                SetVisible(p, false);

        from.group.interactable = false;

        yield return StartCoroutine(Tween.Run(0.18f, Ease.InCubic, t =>
        {
            from.group.alpha = 1f - t;
            from.content.anchoredPosition =
                new Vector2(-dir * Shift * t, 0f);
        }));

        SetVisible(from, false);

        to.group.blocksRaycasts = true;

        yield return StartCoroutine(Tween.Run(0.3f, Ease.OutCubic, t =>
        {
            to.group.alpha = t;
            to.content.anchoredPosition =
                new Vector2(dir * Shift * (1f - t), 0f);
        }));

        SetVisible(to, true);

        busy = false;

        to.onShown.Invoke();
    }

    IEnumerator OverlayIn(ScreenPanel under, ScreenPanel ov)
    {
        busy = true;

        under.group.interactable = false;
        ov.group.blocksRaycasts = true;

        yield return StartCoroutine(Tween.Run(0.28f, Ease.OutBack, t =>
        {
            ov.group.alpha = Mathf.Clamp01(t);

            ov.card.localScale =
                Vector3.one * Mathf.LerpUnclamped(0.85f, 1f, t);
        }));

        SetVisible(ov, true);

        busy = false;

        ov.onShown.Invoke();
    }

    IEnumerator OverlayOut(ScreenPanel ov, ScreenPanel under)
    {
        busy = true;

        ov.group.interactable = false;

        yield return StartCoroutine(Tween.Run(0.18f, Ease.InCubic, t =>
        {
            ov.group.alpha = 1f - t;

            ov.card.localScale =
                Vector3.one * Mathf.LerpUnclamped(1f, 0.92f, t);
        }));

        SetVisible(ov, false);

        under.group.interactable = true;

        busy = false;
    }
}