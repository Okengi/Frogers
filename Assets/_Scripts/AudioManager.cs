using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] sounds;

	private void Awake()
	{
        instance = this;
		foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.loop = sound.loop;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
        }
	}

	void Start()
    {
        PlaySound("Music"); 
    }

    public void PlaySound(string soundName)
    {
        Sound sound = Array.Find<Sound>(sounds, sound => sound.name == soundName);
		if (sound == null) { 
            return; 
        }
		sound.source.Play();
    }
	public void StopSound(string soundName)
	{
		Sound sound = Array.Find<Sound>(sounds, sound => sound.name == soundName);
        if (sound == null) { 
            return; 
        }
		sound.source.Stop();
	}
}
