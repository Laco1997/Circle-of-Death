using UnityEngine.Audio;
using UnityEngine;
using System;
using Unity.VisualScripting;

/*
* Audio manager pre prehravanie a stopnutie SFX a hudby.
*/
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    /*
    * Vytvorenie objektu pre kazdy audio clip s nasledujucimi atributmi.
    */
    void Awake()
    {
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.spatialBlend = s.spatialBlend;
        }   
    }

    /*
    * Najdenie a prehratie audio clipu.
    */
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    /*
    * Najdenie a prehratie audio clipu s delayom.
    */
    public void PlayDelayed(string name, float delay)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.PlayDelayed(delay);
    }

    /*
    * Najdenie a zastavenie audio clipu s delayom.
    */
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }

    /*
    * Ziskanie audio clipu podla nazvu.
    */
    public Sound GetAudioClip(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        return s;
    }
}
