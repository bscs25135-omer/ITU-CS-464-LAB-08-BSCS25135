using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlow : MonoBehaviour
{
    [SerializeField] GameObject pausePanel, settingsPanel;

    public void Pause()
    {
        if (!GameController.Instance.Running || pausePanel.activeSelf) return;
        AudioManager.UI(UiSfx.Pause);
        Time.timeScale = 0f;
        AudioManager.Pause(true);      // SFX pause; music and UI keep playing
        pausePanel.SetActive(true);
    }

    public void Resume()
    {
        AudioManager.UI(UiSfx.Resume);
        settingsPanel.SetActive(false);
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        AudioManager.Pause(false);
    }

    public void OpenSettings() { settingsPanel.SetActive(true); }
    public void CloseSettings() { settingsPanel.SetActive(false); }

    public void Replay()
    {
        Time.timeScale = 1f;
        AudioManager.Pause(false);
        SceneManager.LoadScene("Gameplay");
    }

    public void ToMenu()
    {
        Time.timeScale = 1f;
        AudioManager.Pause(false);
        ScreenManager.StartAtMain = true;
        SceneManager.LoadScene("MainMenu");
    }

    void Update()   // Esc / Android Back
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        if (settingsPanel.activeSelf) CloseSettings();
        else if (pausePanel.activeSelf) Resume();
        else Pause();
    }
}