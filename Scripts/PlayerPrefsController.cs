using UnityEngine;

public static class PlayerPrefsController
{
    private const string SOUND_ON_KEY = "sound on";

    public static int GetSoundOn() { return PlayerPrefs.GetInt(SOUND_ON_KEY, 1); }

    public static void ResetAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    public static void SwitchSoundOn()
    {
        PlayerPrefs.SetInt(SOUND_ON_KEY, GetSoundOn() == 0 ? 1 : 0);
    }

    public static void SetSoundOn(int newSoundOn)
    {
        PlayerPrefs.SetInt(SOUND_ON_KEY, newSoundOn);
    }
}