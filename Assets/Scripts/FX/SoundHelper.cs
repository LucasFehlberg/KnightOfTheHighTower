/*****************************************************************************
// File Name : SoundHelper
// Author : Lucas Fehlberg
// Creation Date : May 11, 2025
// Last Updated : May 11, 2025
//
// Brief Description : SFX controller. For use in animator
*****************************************************************************/

using UnityEngine;

public class SoundHelper : MonoBehaviour
{
    [SerializeField] private AudioSource killSound;
    [SerializeField] private AudioSource hitSound;

    /// <summary>
    /// Plays the hitsound
    /// </summary>
    public void PlayHitSound()
    {
        hitSound.Play();
    }

    /// <summary>
    /// Plays kill sound
    /// </summary>
    public void PlayKillSound()
    {
        killSound.Play();
    }
}
