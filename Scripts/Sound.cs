using UnityEngine;

[CreateAssetMenu(menuName = "Sound")]
public class Sound : ScriptableObject
{
    [SerializeField] AudioClip _clip = null;
    [SerializeField] float _volume = 0.5f;

    public AudioClip Clip => _clip;
    public float Volume => _volume;
}