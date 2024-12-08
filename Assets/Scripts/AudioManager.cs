using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager AudioInst;

    public void PlayAudio(AudioSource Sound)
    {
        Sound.Play();
    }

    public void PauseAudio(AudioSource Sound)
    {
        Sound.Pause();
    }

    public void ResumeAudio(AudioSource Sound)
    {
        Sound.UnPause();
    }

    public void StopAudio(AudioSource Sound)
    {
        Sound.Stop();
    }

}
