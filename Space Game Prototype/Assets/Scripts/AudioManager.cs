using UnityEngine.Audio;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;
    //private AudioSource audioSource;

    public static AudioManager instance; // this is a static ref of the current instance of the AudioManager we have in our scene

    // initialization before the start method is called
    void Awake()
    {
        if (instance == null)       // this if and else call is to make sure the AudioManager instance exists only once in 
            instance = this;        // the scene after loading another scene with the AudioManager, otherwise, two instances
        else                        // will exist due to DontDestroyOnLoad, this falls in category of a "SINGLETON pattern"
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>(); // s.source is referencing the components of the audiosource 
                                                               // from the Sound class we made
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = s.audioMixerGroup;
        }
    }

    public bool FinishedPlaying(string sourceName) {
        foreach(Sound s in sounds) {
            if (s.name == sourceName) {
                Debug.Log("source info: " + s.source.clip.length + " + " + s.source.time + " + " );
                return s.source.time >= s.source.clip.length;
            }
        }

        return false;
    }

    void Start()
    {
        //int x = Random.Range(0, 2);
        //if (x == 0)
        //    Play("Theme");
        //else
        //    Play("Theme2");

        Play("Theme");
    }

    private void Update()
    {
        //foreach (Sound s in sounds)
        //{
        //    if (s.name == "Theme" && s.source.isPlaying == true)
        //    {
        //        break;
        //    }

        //    else (s.name == "Theme" && s.source.isPlaying == false)
        //    {
        //        foreach (Sound s2 in sounds)
        //        {
        //            if (s2.name == "Theme2" && s.source.isPlaying == false)
        //            {
        //                Play("Theme2");
        //            }
        //        }

        //    }

        //    if (s.name == "Theme2" && s.source.isPlaying == true)
        //    {
        //        break;
        //    }

        //    else (s.name == "Theme2" && s.source.isPlaying == false)
        //    {
        //        foreach (Sound s2 in sounds)
        //        {
        //            if (s2.name == "Theme" && s.source.isPlaying == false)
        //            {
        //                Play("Theme");
        //            }
        //        }
        //    }
        //}

    }

    public void Play(string name) 
    {
        Sound s = Array.Find(sounds, sound => sound.name == name); // we could also use a foreach method to look at all the 
                                                                   // soundclips to find the sound with the appopraite name
                                                                   // but this is much simpler and quicker, remember to add
                                                                   // using System; when using Array.Find()
        if (s == null)
        {
            Debug.LogWarning("AudioManager:Sound '" + name + "' not found!"); // used for error when spelling mistake of audioclip
            return;
        }
        s.source.Play();
    } // so now we can play any audioclip with given name from anywhere in our 
      // scripts using FindObjectOfType<AudioManager>().Play("place_name_of_clip_here")

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        s.source.Stop();
    }
}
