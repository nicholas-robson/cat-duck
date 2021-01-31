using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static Player player;

    [SerializeField]
    List<Sound> music;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            player = GetComponentInChildren<Player>();
            DontDestroyOnLoad(this);
        }


        foreach (Sound s in music)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        PlayGenericBackgroundMusic();
    }


    public static void PlayGenericBackgroundMusic()
    {
        _instance.music[0].source.Play();
    }

    public static void StopAllBackgroundMusic()
    {
        foreach(Sound s in _instance.music)
        {
            s.source.Stop();
        }
    }

    public static void Play(string name)
    {
        Sound s = _instance.music.Find(sound => sound.name == name);

        if (s == null)
            return;

        s.source.Play();
    }

    public static void PlayBossBattleMusic()
    {
        _instance.music[1].source.Play();
    }

    private void LoadScene(int doorIndex = 0)
    {
        
    }
}
