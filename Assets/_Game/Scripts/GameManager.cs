using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public Player player;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            player = GetComponentInChildren<Player>();
            DontDestroyOnLoad(this);
        }
    }

    private void Start()
    {
        
    }

    private void LoadScene(int doorIndex = 0)
    {
        
    }
}
