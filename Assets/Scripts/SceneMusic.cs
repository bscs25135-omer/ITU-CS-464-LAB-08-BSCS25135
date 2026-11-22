using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    [SerializeField] AudioClip music;
    void Start() { AudioManager.Music(music); }
}