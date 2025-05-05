/*****************************************************************************
// File Name : SoundController
// Author : Lucas Fehlberg
// Creation Date : May 4, 2025
// Last Updated : May 4, 2025
//
// Brief Description : Attatched to a settings gameObject, contains functions for music sliders
*****************************************************************************/

using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioMixer masterMixer;
    [SerializeField] private Slider musicController;

    /// <summary>
    /// When this is enabled, it will set the music volume to the proper value
    /// </summary>
    private void OnEnable()
    {
        musicController.value = Settings.Data.MusicVolume;
    }

    /// <summary>
    /// Updates the music value
    /// </summary>
    public void UpdateMusicVolume()
    {
        Settings.Data.MusicVolume = musicController.value;
        masterMixer.SetFloat("MusicVolume", Mathf.Log10(Settings.Data.MusicVolume) * 20);
    }
}
