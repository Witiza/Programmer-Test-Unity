using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    static AudioController Instance;
    AudioSource source;
    AudioListener listener;
    public AudioClip game_track;
    public AudioClip menu_track;
    float volume;

    private void Awake()
    {
        SingletonCheck();
    }
    private void SingletonCheck()
    {
        //We check if this GO already exists (which will happen when we restart the game or go to the score screen.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            source = GetComponent<AudioSource>();
            listener = GetComponent<AudioListener>();
            DontDestroyOnLoad(gameObject);
            Instance = this;
            SetTrack(SceneManager.GetActiveScene().buildIndex);
        }
    }
    private void SetTrack(int level)
    {
        volume = SaveLoad.LoadVolume();
        AudioListener.volume = volume;
        if (level == 3)
        {
            source.clip = game_track;
            source.Play();
        }
        else
        {
            if (level == 1)
            {
                GameObject.FindGameObjectWithTag("Volume").GetComponent<Slider>().value = volume;

            }
            if (source.clip.name == "game_track")
            {
                source.clip = menu_track;
                source.Play();
            }

        }
    }
    private void OnLevelWasLoaded(int level)
    {
        //Somehow, this function is called even before awake is called
        SingletonCheck();
    }
    
    public  void ChangeVolume()
    {
        volume = GameObject.FindGameObjectWithTag("Volume").GetComponent<Slider>().value;
        AudioListener.volume = volume;
        //This is ugly, but the other options took too much time or space. plus is late.
        SaveLoad.SaveVolume(volume);
    }
}
