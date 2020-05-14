using System;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    
    public Sound[] sounds;
    
    [Range(0f, 1f)]
    public float baseVolume = .75f;

    private string baseVolumeKey = "baseVolume";

    private float muteVol = 0f;

    public Slider[] volumeModifiers;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            if (PlayerPrefs.HasKey(baseVolumeKey))
            {
                baseVolume = PlayerPrefs.GetFloat(baseVolumeKey);
            }
            Instance = this;
            //DontDestroyOnLoad(gameObject);
            foreach (var s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.loop = s.loop;
                s.source.volume = baseVolume;
            }
        }

        foreach (var slider in volumeModifiers)
        {
            try
            {
                slider.value = baseVolume;
            }
            catch (NullReferenceException ignored) { }
        }
    }

    private void Start()
    {
        Play("theme");
    }

    public void Play(string sound)
    {
        var s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }
    
    // Used in UI
    // ReSharper disable once UnusedMember.Global
    public void setVolume(float volume)
    {
        if (Math.Abs(volume - baseVolume) < 0.001f) return;
        
        var vol = Math.Max(0f, Math.Min(volume, 1f));

        PlayerPrefs.SetFloat(baseVolumeKey, vol);

        // Update Volume
        foreach (var sound in sounds)
        {
            baseVolume = vol;
            sound.source.volume = vol;
        }
        // Update Sliders
        foreach (var slider in volumeModifiers)
        {
            slider.value = baseVolume;
        }
    }

    public void Mute()
    {
        if (Math.Abs(muteVol) < 0.001f)
        {
            muteVol = baseVolume;
            setVolume(0f);
        }
        else
        {
            setVolume(muteVol);
            muteVol = 0f;
        }
    }

    public void playSuccessSound()
    {
        Play("success");
    }

    public void playFailSound()
    {
        Play("fail");
    }
}
