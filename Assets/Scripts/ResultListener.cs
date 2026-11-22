using TMPro;
using UnityEngine;

public class ResultListener : MonoBehaviour
{
    [SerializeField] GameObject resultPanel;
    [SerializeField] TMP_Text title;

    void OnEnable() { GameEvents.GameWon += Won; GameEvents.GameLost += Lost; }
    void OnDisable() { GameEvents.GameWon -= Won; GameEvents.GameLost -= Lost; }

    void Won() { Show("VICTORY"); }
    void Lost() { Show("DEFEAT"); }
    void Show(string text) { title.text = text; resultPanel.SetActive(true); }
}