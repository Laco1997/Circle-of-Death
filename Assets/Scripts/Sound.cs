using UnityEngine.Audio;
using UnityEngine;

/*
* Custom audio clip objekt s roznymi atributmi pre audio manager, 
* ktory je embednuty v editore.
*/
[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;

    public bool loop;

    [Range(0f, 1f)]
    public float spatialBlend;

    [HideInInspector]
    public AudioSource source;

}
