using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    [SerializeField] string sceneName = "Gameplay";
    [SerializeField] Image barFill;
    [SerializeField] TMP_Text percentText;
    bool started;

    public void Begin()      // called from Panel_Loading > On Shown
    {
        if (started) return;
        started = true;
        StartCoroutine(Load());
    }

    IEnumerator Load()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;          // do not enter gameplay yet
        float shown = 0f;
        while (shown < 1f)
        {
            float target = Mathf.Clamp01(op.progress / 0.9f);   // real progress (stops at 0.9)
            shown = Mathf.MoveTowards(shown, target, Time.unscaledDeltaTime * 0.8f);
            barFill.fillAmount = shown;
            percentText.text = Mathf.RoundToInt(shown * 100f) + "%";
            yield return null;
        }
        yield return new WaitForSecondsRealtime(0.2f);
        op.allowSceneActivation = true;           // only now enter gameplay
    }
}