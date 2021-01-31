using System;
using System.Collections.Generic;
using _Game.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static Player player;
    [SerializeField]
    Sound[] music;
    
    public LayerMask cameraRaycastLayerMask;
    public GameObject resetParticles;

    private Camera _camera;
    private RaycastHit[] _raycastHits;
    private List<GameObject> _walls;
    private int _nextEntrance = 0;


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            _camera = GetComponentInChildren<Camera>();
            player = GetComponentInChildren<Player>();
            _raycastHits = new RaycastHit[2];
            _walls = new List<GameObject>();
            
            SceneManager.sceneLoaded += OnSceneLoaded;
            
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

    private static void ResetLevel(Vector3 position)
    {
        player.ResetPosition(position);
        Instantiate(_instance.resetParticles, position, Quaternion.identity);
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

    private void LoadScene(string sceneName, int entranceIndex = 0)
    {
        SceneManager.LoadScene(sceneName);

        _nextEntrance = entranceIndex;
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        ResetLevel(LevelManager.GetEntrance(_nextEntrance).position);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
