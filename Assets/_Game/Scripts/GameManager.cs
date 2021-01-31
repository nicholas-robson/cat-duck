using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static Player player;

    [SerializeField]

    List<Sound> music;

    
    public LayerMask cameraRaycastLayerMask;

    private Camera _camera;
    private RaycastHit[] _raycastHits;
    private List<GameObject> _walls;


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            _camera = GetComponentInChildren<Camera>();
            player = GetComponentInChildren<Player>();
            _raycastHits = new RaycastHit[2];
            _walls = new List<GameObject>();
            
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

    private void LateUpdate()
    {
        var cameraPosition = _camera.transform.position;
        var playerPosition = player.GetPosition();
        var diff = playerPosition - cameraPosition;

        Physics.RaycastNonAlloc(cameraPosition, diff.normalized, _raycastHits, diff.magnitude,
            cameraRaycastLayerMask);

        foreach (var o in GameObject.FindGameObjectsWithTag("Wall"))
        {
            o.transform.Find("WallTall").gameObject.SetActive(true);
            o.transform.Find("WallShort").gameObject.SetActive(false);
        }
        
        foreach (var raycastHit in _raycastHits)
        {
            if (raycastHit.collider == null) continue;
            
            if (!raycastHit.collider.CompareTag("Wall")) continue;
            
            raycastHit.collider.transform.Find("WallTall").gameObject.SetActive(false);
            raycastHit.collider.transform.Find("WallShort").gameObject.SetActive(true);
        }
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
