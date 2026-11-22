using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public enum UiSfx { Hover, Click, Back, Open, Close, Pause, Resume, Success, Error }

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] AudioMixer mixer;
    [SerializeField] AudioSource[] sfxSources;     // 4 sources, output SFX
    [SerializeField] AudioSource uiSource;         // output UI
    [SerializeField] AudioSource musicA, musicB;   // output Music
    [SerializeField] AudioClip[] uiClips;          // in UiSfx enum order
    [SerializeField] float pitchRange = 2f;        // semitones either way
    [SerializeField] float minVolume = 0.85f;
    [SerializeField] float fadeSeconds = 1.5f;

    static readonly string[] Params = { "MasterVolume", "MusicVolume", "SFXVolume", "UIVolume" };
    int next;
    AudioSource current;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        current = musicA;
        // music and UI keep playing while AudioListener.pause is true
        musicA.ignoreListenerPause = true;
        musicB.ignoreListenerPause = true;
        uiSource.ignoreListenerPause = true;
    }

    void Start()
    {
        if (Instance != this) return;
        foreach (string p in Params)
            VolumeSlider.Apply(mixer, p, PlayerPrefs.GetFloat(p, 1f));   // restore saved volumes
    }

    // ---- static helpers, safe to call from anywhere ----
    public static void UI(UiSfx id) { if (Instance != null) Instance.PlayUI(id); }
    public static void Sfx(AudioClip clip, float volume = 1f) { if (Instance != null) Instance.PlaySfx(clip, volume); }
    public static void Music(AudioClip clip) { if (Instance != null) Instance.PlayMusic(clip); }
    public static void Pause(bool paused) { AudioListener.pause = paused; }

    public void PlaySfx(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        AudioSource s = sfxSources[next];
        next = (next + 1) % sfxSources.Length;
        s.pitch = Mathf.Pow(2f, Random.Range(-pitchRange, pitchRange) / 12f);   // pitch variation
        s.PlayOneShot(clip, volume * Random.Range(minVolume, 1f));              // overlapping effects
    }

    public void PlayUI(UiSfx id)
    {
        int i = (int)id;
        if (i >= uiClips.Length || uiClips[i] == null) return;
        uiSource.PlayOneShot(uiClips[i]);
    }

    public void PlayMusic(AudioClip clip)   // equal-power crossfade
    {
        if (clip == null) return;
        if (current.clip == clip && current.isPlaying) return;
        AudioSource to = current == musicA ? musicB : musicA;
        to.clip = clip;
        to.loop = true;
        to.volume = 0f;
        to.Play();
        StopAllCoroutines();
        StartCoroutine(Fade(current, to));
        current = to;
    }

    IEnumerator Fade(AudioSource from, AudioSource to)
    {
        float start = from.volume;
        for (float t = 0f; t < fadeSeconds; t += Time.unscaledDeltaTime)
        {
            float u = t / fadeSeconds;
            from.volume = start * Mathf.Cos(u * Mathf.PI / 2f);
            to.volume = Mathf.Sin(u * Mathf.PI / 2f);
            yield return null;
        }
        from.Stop();
        from.volume = 0f;
        to.volume = 1f;
    }
}