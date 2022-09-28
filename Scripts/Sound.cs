using UnityEngine;

[CreateAssetMenu(menuName = "Sound")]
public class Sound : ScriptableObject
{
    [SerializeField] AudioClip soundClip = null;
    [SerializeField] float soundVolume = 0.5f;

    public AudioClip GetSoundClip() { return soundClip; }
    public float GetSoundVolume() { return soundVolume; }
}