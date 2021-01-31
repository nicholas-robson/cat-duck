using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts
{
    public class LevelManager : MonoBehaviour
    {
        public List<Transform> entrancePlaceholders;
        private static LevelManager _instance;

        private void Awake()
        {
            _instance = this;
        }

        public static Transform GetEntrance(int index)
        {
            return _instance.entrancePlaceholders[index];
        }
    }
}