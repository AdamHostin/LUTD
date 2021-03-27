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
    }

    public void Play(string name)
    {
        Sound s = FindSound(name);
        s.source.PlayOneShot(s.clip);
    }

    public void Stop(string name)
    {
        Sound s = FindSound(name);
        s.source.Stop();
    }

    public Sound FindSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Audio manager invalid sound name: " + name);
            return null;
        }
        else
            return s;
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
}