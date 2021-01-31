using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static Player player;
    [SerializeField]
    Sound[] music;

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


    public void PlayGenericBackgroundMusic()
    {
        music[0].source.Play();
    }

    public void StopAllBackgroundMusic()
    {
        foreach(Sound s in music)
        {
            s.source.Stop();
        }
    }

    public static void Play(string name)
    {
        Sound s = Array.Find(_instance.music, sound => sound.name == name);
        if (s == null)
            return;

        s.source.Play();
    }

    public void PlayBossBattleMusic()
    {
        music[1].source.Play();
    }

    private void LoadScene(int doorIndex = 0)
    {
        
    }
}
