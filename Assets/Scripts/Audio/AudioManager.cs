using UnityEngine.Audio;
using System;
using UnityEngine;

/*  to implement other sfx and music in the game reference steps bellow: 
    1. navigate to audiomanager in scene, add an element to the sound array in inspector 
    2. once adding your element drag that audio clip, and name the clip (adjust volumen and pitch as needed)
    3. in the function you want an audio clip associated with (ex. player death), write the following code      
       FindObjectOfType<AudioManager>().Play(" * ");
       replace * with the name of your clip from the sounds array in audio manager
    4. if you need to stop the audio at some point in another function or part of the script, write the following code     
       FindObjectOfType<AudioManager>().Stop(" * ");
       again, replacing * with the name of your clip from the sounds array
*/ 

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    // create an audiosource for every sound, referencing values set in the inspector
    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume; 
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    // play background music when game starts
    void Start()
    {
        Play("Ambient");
    }

    // call method from outside of the class to play audio clip
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            Debug.LogWarning("sound not find: " + name);
            return;
        }
        if(!s.source.isPlaying)
        {
            s.source.Play();
        }
    }

    // call method from outside of the class to stop audio clip 
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }

    // when game is paused stop audio except ambient music
    public void PauseAll()
    {
        foreach (Sound s in sounds)
        {
            if(s.name == "Walking")
            {
                s.source.Stop();
            }
            else if(s.source.isPlaying && s.name != "Ambient")
            {
                // pause will remember playback position
                s.source.Pause();
            }
        }
    }

    // resume all sfx currently triggered 
    public void ResumeAll()
    {
        foreach (Sound s in sounds)
        {   
            s.source.UnPause();
        }
    }

}
