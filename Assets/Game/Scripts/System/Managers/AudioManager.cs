using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using System;
using Unity.VisualScripting;

[System.Serializable]
public class Sound {
    public string name;
    public AudioClip clip;
    [Range(0, 1)] public float volume = 1;
    [Range(0, 2)] public float pitch = 1;
    public bool loop;
    public bool isMusic;
    public AudioSource source;

    [HideInInspector] public float originalVolume;

    public Sound() {
        volume = 1;
        pitch = 1;
        loop = false;
    }
}


public class AudioManager : MonoBehaviour {

    public static AudioManager Instance;

    public Sound[] Sounds;
    private float masterVolume = 1.0f;
    private float musicVolume = 1.0f;
    private float sfxVolume = 1.0f;


    // --------------------------------------------------------------------
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
            return;
        }

        foreach (Sound s in Sounds) {
            if (!s.source) {
                s.source = gameObject.AddComponent<AudioSource>();
            }

            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.originalVolume = s.volume;
        }

        LoadVolumeSettings();
    }


    // --------------------------------------------------------------------
    public void PlayGameTheme() {
        Sound themeIntro = Array.Find(Sounds, sound => sound.name == "Music_GameThemeIntro");
        Sound themeLoop = Array.Find(Sounds, sound => sound.name == "Music_GameThemeLoop");

        if (themeIntro != null && themeLoop != null) {
            StartCoroutine(FadeIn(themeIntro, 1f));
            themeLoop.source.PlayDelayed(themeIntro.source.clip.length);
        }
    }


    // --------------------------------------------------------------------
    private IEnumerator FadeIn(Sound sound, float duration) {
        sound.source.Play();
        sound.source.volume = 0f;  // Start volume at 0
        float targetVolume = sound.originalVolume * musicVolume * masterVolume;
        float startTime = Time.time;

        while (Time.time < startTime + duration) {
            sound.source.volume = Mathf.Lerp(0, targetVolume, (Time.time - startTime) / duration);
            yield return null;
        }

        sound.source.volume = targetVolume;
    }


    // --------------------------------------------------------------------
    private IEnumerator FadeOut(Sound sound, float duration) {
        float startVolume = sound.source.volume;
        float startTime = Time.time;

        while (Time.time < startTime + duration) {
            sound.source.volume = Mathf.Lerp(startVolume, 0, (Time.time - startTime) / duration);
            yield return null;
        }

        sound.source.volume = 0;
        sound.source.Pause();
    }


    // --------------------------------------------------------------------
    public void Play(string name) {
        Sound s = Array.Find(Sounds, sound => sound.name == name);
        if (s == null) {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }
        s.source.Play();
    }


    // --------------------------------------------------------------------
    public void Stop(string name) {
        Sound s = Array.Find(Sounds, sound => sound.name == name);
        s.source.Stop();
    }


    // --------------------------------------------------------------------
    public void Pause() {
        foreach (Sound s in Sounds) {
            if (s.source.isPlaying) {
                s.source.Pause();
            }
        }
    }


    // --------------------------------------------------------------------
    public void UnPause() {
        foreach (Sound s in Sounds) {
            s.source.UnPause();
        }
    }


    // --------------------------------------------------------------------
    public bool IsPlaying(string name) {
        Sound s = Array.Find(Sounds, sound => sound.name == name);
        return s.source.isPlaying;
    }


    // --------------------------------------------------------------------
    public void SetMasterVolume(float volume) {
        masterVolume = volume;
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        UpdateVolumes();
    }


    // --------------------------------------------------------------------
    public void SetMusicVolume(float volume) {
        musicVolume = volume;
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        UpdateVolumes();
    }


    // --------------------------------------------------------------------
    public void SetSFXVolume(float volume) {
        sfxVolume = volume;
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        UpdateVolumes();
    }


    // --------------------------------------------------------------------
    private void UpdateVolumes() {
        foreach (Sound s in Sounds) {
            if (s.isMusic) {
                s.source.volume = s.originalVolume * musicVolume * masterVolume;
            }
            else {
                s.source.volume = s.originalVolume * sfxVolume * masterVolume;
            }
        }
    }


    // --------------------------------------------------------------------
    private void LoadVolumeSettings() {
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
        UpdateVolumes();
    }
}
