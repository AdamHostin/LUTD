using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public AudioMixer mainMixer;
    public Sound[] sounds;
    public Sound[] triggerIndependentSounds;
    public float minTimeBetweenAmbient;
    public float maxTimeBetweenAmbient;

    Sound currentAmbient;

    AudioSource pauseSound;
    AudioSource menuSound;
    AudioSource levelFailedSound;

    private void Awake()
    {
        App.audioManager = this;

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.output;
            s.source.volume = s.volume;
            s.source.loop = s.loop;

            if (s.name == "PauseSound")
                pauseSound = s.source;
            if (s.name == "MenuSound")
                menuSound = s.source;
            if (s.name == "LevelFailed")
                levelFailedSound = s.source;
        }

        foreach (Sound s in triggerIndependentSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.output;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        LoadVolumeValues();
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Audio manager invalid sound name: " + name);
            return;
        }
        s.source.PlayOneShot(s.clip);
    }

    public IEnumerator PlayAmbient()
    {
        Debug.Log("Ambient played");

        while (true)
        {
            currentAmbient = triggerIndependentSounds[UnityEngine.Random.Range(0, triggerIndependentSounds.Length)];
            currentAmbient.source.Play();
            yield return new WaitForSeconds(currentAmbient.source.clip.length + UnityEngine.Random.Range(minTimeBetweenAmbient, maxTimeBetweenAmbient)); 
        }
    }

    public void StopAmbient()
    {
        Debug.Log("Ambient stopped");

        StopCoroutine(PlayAmbient());
        currentAmbient.source.Stop();
    }

    void LoadVolumeValues()
    {
        LoadValue("masterVol");
        LoadValue("sfxVol");
        LoadValue("ambientVol");
    }

    void LoadValue(string key)
    {
        if (PlayerPrefs.HasKey(key))
            mainMixer.SetFloat(key, Mathf.Log10(Mathf.Max(PlayerPrefs.GetFloat(key), 0.0001f)) * 20f);
        else
            mainMixer.SetFloat(key, Mathf.Log10(0.5f) * 20f);
    }

    public void PlayPauseSound()
    {
        pauseSound.Play();
    }

    public void StopPauseSound()
    {
        pauseSound.Stop();
    }

    public void PlayMenuSound()
    {
        menuSound.Play();
    }

    public void StopMenuSound()
    {
        menuSound.Stop();
    }

    public void StopLevelFailedSound()
    {
        if (levelFailedSound.isPlaying)
            levelFailedSound.Stop();
    }
}