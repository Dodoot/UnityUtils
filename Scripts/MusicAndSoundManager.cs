using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicAndSoundManager : MonoBehaviour
{
    // Parameters
    [SerializeField] Sound[] soundsArray = null;

    // Variables and cached references
    string[] availableSoundNames = null;
    AudioSource myAudioSource = null;
    float initialMusicVolume = 0.5f;

    static MusicAndSoundManager instance;

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
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
        else if (allManagers.Length == 1)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
        initialMusicVolume = myAudioSource.volume;

        AdjustMusicVolume();

        availableSoundNames = new string[soundsArray.Length];
        for (int i = 0; i < soundsArray.Length; i++)
        {
            availableSoundNames[i] = soundsArray[i].name;
        }
    }

    // Public methods
    public void TriggeredUpdate()
    {
        AdjustMusicVolume();
    }

    public static void PlayMusic()
    {
        instance.myAudioSource.Play();
    }

    public static void ResetMusic()
    {
        instance.myAudioSource.Stop();
    }

    public static void FadeOutMusic()
    {
        instance.StartCoroutine(instance.FadeOutMusicCoroutine());
    }

    private IEnumerator FadeOutMusicCoroutine()
    {
        var timer = 5f;
        var initialVolume = myAudioSource.volume;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            myAudioSource.volume = timer / 5f * initialVolume;
            yield return new WaitForEndOfFrame();
        }
    }

    public static void PlaySound(string soundToPlayName)
    {
        int soundIndex = Array.IndexOf(instance.availableSoundNames, soundToPlayName);

        if (soundIndex == -1) { Debug.Log("No sound named: " + soundToPlayName); }
        else
        {
            Sound soundToPlay = instance.soundsArray[soundIndex];
            bool isSoundOn = PlayerPrefsController.GetSoundOn() != 0;
            float volumeToPlay = isSoundOn ? soundToPlay.GetSoundVolume() : 0f;
            AudioSource.PlayClipAtPoint(
                soundToPlay.GetSoundClip(),
                FindObjectOfType<AudioListener>().transform.position,
                volumeToPlay);
        }
    }

    // Private methods
    private void AdjustMusicVolume()
    {
        bool isSoundOn = PlayerPrefsController.GetSoundOn() != 0;
        if (isSoundOn)
        {
            myAudioSource.volume = initialMusicVolume;
        }
        else
        {
            myAudioSource.volume = 0f;
        }
    }
}