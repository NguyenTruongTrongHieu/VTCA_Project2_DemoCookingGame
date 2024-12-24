using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager audioInstance;

    public Sound[] musics;
    public Sound[] sfx;

    public AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    VolumeSetting volumeSetting;

    private void Awake()
    {
        if (audioInstance == null)
        {
            audioInstance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        { 
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayMusic(string name)
    {
        var music = Array.Find(musics, x => x.name == name);

        if (music == null)
        {
            Debug.Log("Khong tim thay am thanh");
            return;
        }

        musicSource.clip = music.audioClip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(string name)
    {
        var soundEffect = Array.Find(sfx, x => x.name == name);

        if (soundEffect == null)
        {
            Debug.Log("Khong tim thay am thanh");
            return;
        }

        sfxSource.PlayOneShot(soundEffect.audioClip);
    }
}
