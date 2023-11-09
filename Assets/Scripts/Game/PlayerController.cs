using Project.Managing;
using UnityEngine;

namespace Project.Game
{
    public class PlayerController : MonoBehaviour
    {
        private const int JoystickBorder = 2;

        [SerializeField] private DynamicJoystick _joystick;
        [SerializeField] private GameObject _playerBody;
        [SerializeField] private GameObject _playerRagdoll;
        [SerializeField] private GameObject _playerObject;
        [SerializeField] private TrailRenderer _trailRenderer;
        [SerializeField] private float _speed = 8f;

        private bool _isGameRunning;
        private float _moveInputHorizontal;

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

        private void OnGameStart(bool isGameRunning)
        {
            _isGameRunning = isGameRunning;
        }

        private void OnGameEnd(bool isGameRunning)
        {
            _isGameRunning = isGameRunning;
            RagdollAnimation();
            TrailAnimation();
        }

        private void Update()
        {
            if (!_isGameRunning) return;
            PlayerMovement();

        }

        private void PlayerMovement()
        {
            _playerBody.transform.Translate(Vector3.forward * _speed * Time.deltaTime);
            _moveInputHorizontal = _joystick.Horizontal * JoystickBorder;
            transform.position = new Vector3(_moveInputHorizontal, transform.position.y, transform.position.z);
        }

        private void RagdollAnimation()
        {
            _playerObject.GetComponent<Rigidbody>().mass = 1.0f;
            _playerObject.GetComponent<Animator>().enabled = false;
            _playerRagdoll.SetActive(true);
            _playerObject.GetComponent<Rigidbody>().AddForce(Vector3.forward * 1000, ForceMode.Impulse);
        }

        private void TrailAnimation()
        {
            _trailRenderer.time = 100;
        }
    }
}