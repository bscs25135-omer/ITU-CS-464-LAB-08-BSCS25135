using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiSoundBinder : MonoBehaviour
{
    void Start()
    {
        foreach (Button b in GetComponentsInChildren<Button>(true)) Bind(b);
        foreach (Toggle t in GetComponentsInChildren<Toggle>(true))
            t.onValueChanged.AddListener(_ => AudioManager.UI(UiSfx.Click));
    }

    void Bind(Button b)
    {
        UiSfx? click = Pick(b);
        if (click.HasValue) b.onClick.AddListener(() => AudioManager.UI(click.Value));
        EventTrigger trigger = b.GetComponent<EventTrigger>();
        if (trigger == null) trigger = b.gameObject.AddComponent<EventTrigger>();
        var entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        entry.callback.AddListener(_ => AudioManager.UI(UiSfx.Hover));
        trigger.triggers.Add(entry);
    }

    // Pause and Resume sounds are played by GameFlow, so they return null here.
    static UiSfx? Pick(Button b)
    {
        string n = b.name;
        TMP_Text t = b.GetComponentInChildren<TMP_Text>();
        string label = t != null ? t.text.Trim().ToUpperInvariant() : "";
        if (n.Contains("Pause") || n.Contains("Resume")) return null;
        if (n.Contains("Back") || n.Contains("Done") || n.Contains("Quit") || n.Contains("Menu")) return UiSfx.Back;
        if (label.StartsWith("LEVEL") || label == "LOCKED" || label == "OWNED") return UiSfx.Error;
        if (label.Length > 0 && int.TryParse(label, out _)) return UiSfx.Success;
        return UiSfx.Click;
    }
}