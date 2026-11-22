using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VolumeSlider : MonoBehaviour, IPointerUpHandler
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] string parameter = "MusicVolume";
    [SerializeField] TMP_Text label;
    Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();
        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider.wholeNumbers = false;
        slider.SetValueWithoutNotify(PlayerPrefs.GetFloat(parameter, 1f));
        slider.onValueChanged.AddListener(Set);
        Set(slider.value);
    }

    public void Set(float value)
    {
        Apply(mixer, parameter, value);
        PlayerPrefs.SetFloat(parameter, value);
        if (label != null) label.text = Mathf.RoundToInt(value * 100f) + "%";
    }

    // dB = 20 * log10(volume); 0 is mapped to -80 dB so we never take log(0)
    public static void Apply(AudioMixer m, string param, float value)
    {
        float db = value <= 0.0001f ? -80f : Mathf.Log10(value) * 20f;
        m.SetFloat(param, db);
    }

    public void OnPointerUp(PointerEventData e) { PlayerPrefs.Save(); }
    void OnApplicationPause(bool paused) { if (paused) PlayerPrefs.Save(); }
    void OnDestroy() { PlayerPrefs.Save(); }
}