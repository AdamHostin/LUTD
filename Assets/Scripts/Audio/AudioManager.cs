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
        StartCoroutine(PlayAmbient());
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Audio manager invalid sound name: " + name);
            return;
        }
        s.source.Play();
    }

    IEnumerator PlayAmbient()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(minTimeBetweenAmbient, maxTimeBetweenAmbient));
            triggerIndependentSounds[UnityEngine.Random.Range(0, triggerIndependentSounds.Length)].source.Play();
        }
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
            mainMixer.SetFloat(key, Mathf.Log10(PlayerPrefs.GetFloat(key)) * 20);
    }
}