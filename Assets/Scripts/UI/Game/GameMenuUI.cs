using Project.Managing;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.UI.Game
{    /// <summary>
     /// Handles the display and functionality of the in-game menu UI, such as starting and ending the game,
     /// reloading the scene, and returning to the main menu.
     /// </summary>
    public class GameMenuUI : MonoBehaviour
    {
        [SerializeField] private GameObject _startMenu;
        [SerializeField] private GameObject _endMenu;

        private bool _isFirstTouch = false;

        private void Awake()
        {
            _startMenu.SetActive(true);
            _endMenu.SetActive(false);
            Application.targetFrameRate = 60;
        }

        private void OnEnable()
        {
            GameEventManager.Instance.OnGameStart.AddListener(OnGameStart);
            GameEventManager.Instance.OnGameEnd.AddListener(OnGameEnd);
        }

        private void OnDisable()
        {
            GameEventManager.Instance.OnGameStart.RemoveListener(OnGameStart);
            GameEventManager.Instance.OnGameEnd.RemoveListener(OnGameEnd);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !_isFirstTouch)
            {
                GameEventManager.Instance.StartGame();
            }
        }

        public void ReloadScene()
        {
            SceneManager.LoadSceneAsync("GameScene");
        }

        public void LoadMenu()
        {
            SceneManager.LoadSceneAsync("MenuScene");
        }

        private void OnGameStart(bool isGameRunning)
        {
            Time.timeScale = 1.5f;
            _isFirstTouch = true;
            _startMenu.SetActive(false);
        }

        private void OnGameEnd(bool isGameRunning)
        {
            _endMenu.SetActive(true);
            Time.timeScale = 1.0f;
        }
    }
}