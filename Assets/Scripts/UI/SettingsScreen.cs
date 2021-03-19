﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsScreen : ScreenBase
{
    public AudioMixer mainMixer;

    public Slider mainSlider;
    public Slider sfxSlider;
    public Slider ambientSlider;

    public override void Show()
    {
        base.Show();
        LoadValue("masterVol", mainSlider);
        LoadValue("sfxVol", sfxSlider);
        LoadValue("ambientVol", ambientSlider);
    }

    public override void Hide()
    {
        PlayerPrefs.SetFloat("masterVol", mainSlider.value);
        PlayerPrefs.SetFloat("sfxVol", sfxSlider.value);
        PlayerPrefs.SetFloat("ambientVol", ambientSlider.value);

        base.Hide();
    }

    public void SetMainVolume()
    {
        mainMixer.SetFloat("masterVol", Mathf.Log10(mainSlider.value) * 20);
    }

    public void SetSFXVolume()
    {
        mainMixer.SetFloat("sfxVol", Mathf.Log10(sfxSlider.value) * 20);
    }

    public void SetAmbientVolume()
    {
        mainMixer.SetFloat("ambientVol", Mathf.Log10(ambientSlider.value) * 20);
    }

    void LoadValue(string key, Slider slider)
    {
        if (PlayerPrefs.HasKey(key))
        {
            slider.value = PlayerPrefs.GetFloat(key);
        }
    }
}