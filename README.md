# EMBERFALL Lab 08: Loading, Audio and Events

Omer Abdullah | BSCS25135 | Unity 6000.3.24f1 (Universal 3D)

**Demo video:** https://drive.google.com/file/d/1dV7YOuR-wDzAyMJ33smZa5u2M-QXLJ9F/view?usp=sharing

Built on my Lab 07 project: all 10 screens, tweens and layout are kept.

## Setup
1. git clone https://github.com/bscs25135-omer/ITU-CS-464-LAB-08-BSCS25135.git
2. Unity Hub > Add project from disk, open with 6000.3.24f1.
3. Open Assets/Scenes/MainMenu and press Play (always start here).

## Loading flow
Legends > START MATCH > Panel_Loading. LoadingManager calls SceneManager.LoadSceneAsync("Gameplay") with allowSceneActivation = false. The bar shows op.progress / 0.9 (real progress) and the scene is activated only when it reaches 100%.

## Audio Mixer groups
GameAudioMixer: Master > Music, SFX, UI. Exposed parameters: MasterVolume, MusicVolume, SFXVolume, UIVolume.

| AudioSource | Group |
|---|---|
| MusicA, MusicB | Music |
| Sfx1 to Sfx4 (gameplay effects) | SFX |
| UiSource (hover, click, back, panels, pause, resume, success, error) | UI |

![Audio Mixer](docs/audio-mixer.png)

## Volume mapping and persistence
dB = 20 x log10(value); a slider value of 0 is mapped to -80 dB (mute, never log(0)).

| Slider | dB |
|---|---|
| 1.0 | 0 |
| 0.5 | -6.02 |
| 0.25 | -12.04 |
| 0.1 | -20 |
| 0 | -80 |

Values are saved with PlayerPrefs (key = parameter name), saved when the slider is released and when the app pauses, and re-applied by AudioManager at start.

## Music transitions
SceneMusic calls AudioManager.PlayMusic. Two sources crossfade with an equal-power curve (1.5 s). Menu loop to gameplay loop on entering Gameplay, and back to the menu loop on return.

## Audio while paused
On pause AudioListener.pause = true. Music and the UI source use ignoreListenerPause, so music and pause/resume/UI sounds keep playing; gameplay SFX stop with the game. Settings can be changed live from the pause menu.

## C# events

| Event | Publisher | Subscribers |
|---|---|---|
| GameStarted(max, goal) | GameController | HudListener |
| PlayerDamaged(current, max) | GameController | HudListener (health bar), HitFlashListener (screen flash), GameSfxListener (damage sound) |
| OrbCollected(score, goal) | GameController | HudListener (score), GameSfxListener (pickup sound) |
| GameWon / GameLost | GameController | GameSfxListener (win/lose sound), ResultListener (result screen) |

```csharp
// GameEvents.cs: declaration and safe invocation
public static event Action<int, int> PlayerDamaged;
public static void RaisePlayerDamaged(int current, int max) { PlayerDamaged?.Invoke(current, max); }

// GameController.cs: publisher
GameEvents.RaisePlayerDamaged(health, maxHealth);

// HudListener.cs: subscribe and unsubscribe pair
void OnEnable()  { GameEvents.PlayerDamaged += OnDamaged; }
void OnDisable() { GameEvents.PlayerDamaged -= OnDamaged; }
```

## Gameplay and UI sounds
Pickup, damage, win and lose use AudioManager.PlaySfx (PlayOneShot with +/-2 semitone pitch variation and slight volume variation). UI sounds use one reusable AudioManager UI source driven by UiSoundBinder (no AudioSource per button).

## Audio credits

| File | Title | Author | Source URL | Licence |
|---|---|---|---|---|
| ui_hover.wav | UI Hover Micro Swish | Qvantor | https://freesound.org/people/Qvantor/sounds/878248/ | CC0 |
| ui_click.wav | Low_Hum_UI_Click_38 | SignatureSoundsOrg | https://freesound.org/people/SignatureSoundsOrg/sounds/881019/ | CC0 |
| ui_back.wav | Button Click 1.wav | Mellau | https://freesound.org/people/Mellau/sounds/506054/ | CC0 |
| ui_open.wav | WHOOSH 1 | sound.variant | https://freesound.org/people/sound.variant/sounds/807304/ | CC0 |
| ui_close.wav | UI Button Sound (Cancel / Back / Exit) | Nomagician | https://freesound.org/people/Nomagician/sounds/833628/ | CC0 |
| ui_pause.wav | Pause Game (Gentle Echoes) 1 | SilverIllusionist | https://freesound.org/people/SilverIllusionist/sounds/836224/ | CC0 |
| ui_resume.wav | Resume | MATUSTRM | https://freesound.org/people/MATUSTRM/sounds/836022/ | CC0 |
| ui_success.wav | UI Toast Notification | Qvantor | https://freesound.org/people/Qvantor/sounds/878254/ | CC0 |
| ui_error.wav | 8-Bit Error Buzz | Qvantor | https://freesound.org/people/Qvantor/sounds/878325/ | CC0 |
| sfx_pickup.wav | Cozy UI Reward Coin 01 | yetiancai_audio | https://freesound.org/people/yetiancai_audio/sounds/875917/ | CC0 |
| sfx_damage.wav | 8-Bit Hit | Qvantor | https://freesound.org/people/Qvantor/sounds/878322/ | CC0 |
| sfx_win.wav | sfx_win_3.wav | Mihacappy | https://freesound.org/people/Mihacappy/sounds/844146/ | CC0 |
| sfx_lose.wav | Casual Death Loose | GameAudio | https://freesound.org/people/GameAudio/sounds/220203/ | CC0 |
| music_menu.wav | Background music menu screen | MreFF | https://freesound.org/people/MreFF/sounds/818652/ | CC0 |
| music_game.wav | 80s game music | brad91 | https://freesound.org/people/brad91/sounds/516880/ | CC0 |

No copyrighted commercial game audio was used.
```