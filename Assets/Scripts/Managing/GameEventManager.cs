using UnityEngine;
using UnityEngine.Events;

namespace Project.Managing
{
    public class GameEventManager : MonoBehaviour
    {
        public static GameEventManager Instance;

        public GameEvent OnGameStart;
        public GameEvent OnGameEnd;
        public GameEvent OnAddNewCube;
        public GameEvent OnCollisionWall;

        private bool _isGameRunning;

        [System.Serializable]
        public class GameEvent : UnityEvent<bool> { }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        public void StartGame()
        {
            if (!_isGameRunning)
            {
                _isGameRunning = true;
                OnGameStart?.Invoke(true);
            }
        }

        public void EndGame()
        {
            if (_isGameRunning)
            {
                _isGameRunning = false;
                OnGameEnd?.Invoke(false);
            }
        }

        public void AddNewCube()
        {
            if (_isGameRunning)
            {
                OnAddNewCube?.Invoke(true);
            }
        }

        public void CollisionWall()
        {
            if (_isGameRunning)
            {
                OnCollisionWall?.Invoke(true);
            }
        }
    }
}