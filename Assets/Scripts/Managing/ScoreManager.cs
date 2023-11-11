using TMPro;
using UnityEngine;

namespace Project.Managing
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textField;

        private int score;

        private void OnEnable()
        {
            score = 0;
            _textField.text = " Score: " + score.ToString();

            GameEventManager.Instance.OnAddNewCube.AddListener(AddScore);
            GameEventManager.Instance.OnCollisionWall.AddListener(AddScore);
            GameEventManager.Instance.OnGameEnd.AddListener(TrySetRecord);
        }

        private void OnDisable()
        {
            GameEventManager.Instance.OnAddNewCube.RemoveListener(AddScore);
            GameEventManager.Instance.OnCollisionWall.RemoveListener(AddScore);
            GameEventManager.Instance.OnGameEnd.RemoveListener(TrySetRecord);
        }

        private void AddScore(bool isGameRunning)
        {
            score += 5;

            _textField.text = " Score: " + score.ToString();
        }

        private void TrySetRecord(bool isGameRunning)
        {
            GameManager.Instance.DatabaseController.TrySetNewRecord(score);
        }
    }
}