using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicAndSoundManager : MonoBehaviour
{
    // Serialize Fields
    [SerializeField] private Sound[] _sounds = null;

    // Privates
    private string[] _availableSoundNames = null;
    private AudioSource _audioSource = null;
    private AudioListener _audioListener = null;
    private float _initialMusicVolume = 0.5f;

    // Static
    private static MusicAndSoundManager _instance;

    // Unity methods
    private void Awake()
    {
        // custom singleton pattern that keeps the same if music is the same
        MusicAndSoundManager[] allManagers = FindObjectsOfType<MusicAndSoundManager>();
        if (allManagers.Length > 2)
        {
            Destroy(gameObject);
            Debug.LogError("Too many music managers");
        }
        else if (allManagers.Length == 2)
        {
            if (allManagers[0].GetComponent<AudioSource>().clip.ToString()
                == allManagers[1].GetComponent<AudioSource>().clip.ToString())
            {
                Destroy(gameObject);
            }
            else
            {
                foreach (MusicAndSoundManager manager in allManagers)
                {
                    if (manager.GetComponent<AudioSource>().clip.ToString()
                        != GetComponent<AudioSource>().clip.ToString())
                    {
                        Destroy(manager.gameObject);
                    }
                }
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
        else if (allManagers.Length == 1)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _initialMusicVolume = _audioSource.volume;

        AdjustMusicVolume();

        _availableSoundNames = new string[_sounds.Length];
        for (int i = 0; i < _sounds.Length; i++)
        {
            _availableSoundNames[i] = _sounds[i].name;
        }
    }

    // Public static methods
    public static void TriggeredUpdate()
    {
        _instance.AdjustMusicVolume();
    }

    public static void PlayMusic()
    {
        _instance._audioSource.Play();
    }

    public static void StopMusic()
    {
        _instance._audioSource.Stop();
    }

    public static void FadeOutMusic(float time)
    {
        _instance.StartCoroutine(_instance.FadeMusicCoroutine(0, time));
    }

    public static void FadeInMusic(float time)
    {
        _instance.StartCoroutine(_instance.FadeMusicCoroutine(_instance._initialMusicVolume, time));
    }

    public static void PlaySound(string soundName)
    {
        if (_instance._audioListener == null)
        {
            _instance._audioListener = FindObjectOfType<AudioListener>();
        }

        PlaySound(soundName, _instance._audioListener.transform.position);
    }

    public static void PlaySound(string soundName, Vector3 position)
    {
        int soundIndex = Array.IndexOf(_instance._availableSoundNames, soundName);

        if (soundIndex == -1)
        {
            Debug.Log("No sound named: " + soundName);
        }
        else
        {
            Sound sound = _instance._sounds[soundIndex];
            float volume = PlayerPrefsController.GetSoundOn() != 0 ? sound.Volume : 0f;

            AudioSource.PlayClipAtPoint(sound.Clip, position, volume);
        }
    }

    // Private methods
    private void AdjustMusicVolume()
    {
        if (PlayerPrefsController.GetSoundOn() != 0)
        {
            _audioSource.volume = _initialMusicVolume;
        }
        else
        {
            _audioSource.volume = 0f;
        }
    }

    private IEnumerator FadeMusicCoroutine(float targetVolume, float time)
    {
        var timer = 0f;
        var initialVolume = _audioSource.volume;

        while (timer < time)
        {
            timer += Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(initialVolume, targetVolume, timer / time);
            yield return new WaitForEndOfFrame();
        }
    }
}