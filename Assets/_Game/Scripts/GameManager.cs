using System;
using System.Collections.Generic;
using _Game.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static Player player;

    [SerializeField]

    List<Sound> music;

    
    public LayerMask cameraRaycastLayerMask;
    public GameObject resetParticles;
    public float sphereCastRadius = 3f;

    private Camera _camera;
    private RaycastHit[] _raycastHits;
    private List<GameObject> _walls;
    private int _nextEntrance = 0;

    [SerializeField]
    TextMeshProUGUI controls;
    [SerializeField]
    TextMeshProUGUI hint;

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
            
            foreach (Sound s in music)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;

                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
            }
            
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(GetComponentInChildren<Player>().gameObject);
        }
    }

    private void Start()
    {
        PlayGenericBackgroundMusic();
        StartCoroutine(DisableControls());
    }

    public static void ResetLevel(int entranceIndex = 0)
    {
        var position = LevelManager.GetEntrance(entranceIndex).position;
        player.SetPosition(position);
        Instantiate(_instance.resetParticles, position, Quaternion.identity);
    }
    
    private void LateUpdate()
    {
        var cameraPosition = _camera.transform.position;
        var playerPosition = player.GetPosition();
        
        // Account for spherecast diameter.
        playerPosition -= (playerPosition - cameraPosition).normalized * (sphereCastRadius * 2f);
        
        var diff = playerPosition - cameraPosition;
        
        foreach (var raycastHit in _raycastHits)
        {
            if (raycastHit.collider == null) continue;
            
            if (!raycastHit.collider.CompareTag("Wall")) continue;
            
            raycastHit.collider.transform.Find("WallTall").gameObject.SetActive(true);
            raycastHit.collider.transform.Find("WallShort").gameObject.SetActive(false);
        }

        _raycastHits = Physics.SphereCastAll(cameraPosition, sphereCastRadius, diff.normalized, diff.magnitude,
            cameraRaycastLayerMask);

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

    public static void LoadScene(string sceneName, int entranceIndex = 0)
    {
        SceneManager.LoadScene(sceneName);

        _instance._nextEntrance = entranceIndex;
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private static void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        ResetLevel(_instance._nextEntrance);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    IEnumerator DisableControls()
    {
        yield return new WaitForSeconds(10);
        controls.enabled = false;
        StartCoroutine(EnableDisableHint());
    }

    IEnumerator EnableDisableHint()
    {
        yield return new WaitForSeconds(2);
        hint.gameObject.SetActive(true);
        yield return new WaitForSeconds(5);
        hint.gameObject.SetActive(false);

    }

}
