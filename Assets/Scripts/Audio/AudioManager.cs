using UnityEngine.Audio;
using System;
using UnityEngine;

/*  to implement other sfx and music in the game reference steps bellow: 
    1. navigate to audiomanager in scene, add an element to the sound array in inspector 
    2. once adding your element drag that audio clip, and name the clip (adjust volumen and pitch as needed)
    3. in the function you want an audio clip associated with (ex. player death), write the following code 
       FindObjectOfType<AudioManager>().Play(" * ");
       replace * with the name of your clip from the sounds array in audio manager
*/ 

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    void Awake(){
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume; 
            s.source.pitch = s.pitch;
        }
    }

    void Start()
    {
        // play background music when game starts
        Play("Ambient");
    }

    // call method from outside of the class
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }
}
