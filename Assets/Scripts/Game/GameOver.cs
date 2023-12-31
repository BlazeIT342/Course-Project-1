﻿using Project.Managing;
using UnityEngine;

namespace Project.Game
{
    /// <summary>
    /// Monitors collisions and signals the end of the game when colliding with objects tagged as "Wall."
    /// </summary>
    public class GameOver : MonoBehaviour
    {
        private GameEventManager _gameEventManager;

        private void OnEnable()
        {
            _gameEventManager = GameEventManager.Instance;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Wall"))
            {
                _gameEventManager.EndGame();
            }
        }
    }
}