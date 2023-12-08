using TMPro;
using UnityEngine;

namespace Project.Managing
{
    /// <summary>
    /// Manages the scoring system in the game, updating the _score based on game events and attempting to set a new record.
    /// </summary>
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textField;
        private GameEventManager _gameEventManager;
        private DatabaseManager _databaseManager;
        private int _score;

        private void OnEnable()
        {
            _score = 0;
            _textField.text = " Score: " + _score.ToString();

            _databaseManager = DatabaseManager.Instance;
            _gameEventManager = GameEventManager.Instance;

            _gameEventManager.OnAddNewCube.AddListener(AddScore);
            _gameEventManager.OnCollisionWall.AddListener(AddScore);
            _gameEventManager.OnGameEnd.AddListener(TrySetRecord);
        }

        private void OnDisable()
        {
            _gameEventManager.OnAddNewCube.RemoveListener(AddScore);
            _gameEventManager.OnCollisionWall.RemoveListener(AddScore);
            _gameEventManager.OnGameEnd.RemoveListener(TrySetRecord);
        }

        private void AddScore(bool isGameRunning)
        {
            _score += 5;

            _textField.text = " Score: " + _score.ToString();
        }

        private void TrySetRecord(bool isGameRunning)
        {
            _databaseManager.DatabaseController.TrySetNewRecord(_score);
        }
    }
}