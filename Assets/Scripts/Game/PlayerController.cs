using Project.Managing;
using UnityEngine;

namespace Project.Game
{
    /// <summary>
    /// Controls the player character's movement, animation transitions, and trail effects during gameplay.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        private const int JoystickBorder = 2; // The border value for joystick input.

        [SerializeField] private DynamicJoystick _joystick;       // Reference to the dynamic joystick for player control.
        [SerializeField] private GameObject _playerBody;          // Reference to the main character's body.
        [SerializeField] private GameObject _playerRagdoll;       // Reference to the player's ragdoll object.
        [SerializeField] private GameObject _playerObject;        // Reference to the entire player object.
        [SerializeField] private TrailRenderer _trailRenderer;     // Reference to the trail renderer for visual effects.
        [SerializeField] private float _speed = 8f;               // The speed of the player character.

        private bool _isGameRunning;               // Indicates whether the game is currently running.
        private float _moveInputHorizontal;        // Horizontal input value for player movement.
        private GameEventManager _gameEventManager;

        private void OnEnable()
        {
            _gameEventManager = GameEventManager.Instance;

            _gameEventManager.OnGameStart.AddListener(OnGameStart);
            _gameEventManager.OnGameEnd.AddListener(OnGameEnd);
        }

        private void OnDisable()
        {
            _gameEventManager.OnGameStart.RemoveListener(OnGameStart);
            _gameEventManager.OnGameEnd.RemoveListener(OnGameEnd);
        }

        /// <summary>
        /// Handles initialization logic when the game starts.
        /// </summary>
        private void OnGameStart(bool isGameRunning)
        {
            _isGameRunning = isGameRunning;
        }

        /// <summary>
        /// Handles actions when the game ends, such as triggering ragdoll animation and adjusting trail effects.
        /// </summary>
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

        /// <summary>
        /// Handles player movement based on joystick input.
        /// </summary>
        private void PlayerMovement()
        {
            _playerBody.transform.Translate(Vector3.forward * _speed * Time.deltaTime);
            _moveInputHorizontal = _joystick.Horizontal * JoystickBorder;
            transform.position = new Vector3(_moveInputHorizontal, transform.position.y, transform.position.z);
        }

        /// <summary>
        /// Triggers the ragdoll animation effect when the game ends.
        /// </summary>
        private void RagdollAnimation()
        {
            _playerObject.GetComponent<Rigidbody>().mass = 1.0f;
            _playerObject.GetComponent<Animator>().enabled = false;
            _playerRagdoll.SetActive(true);
            _playerObject.GetComponent<Rigidbody>().AddForce(Vector3.forward * 1000, ForceMode.Impulse);
        }

        /// <summary>
        /// Adjusts trail effects when the game ends.
        /// </summary>
        private void TrailAnimation()
        {
            _trailRenderer.time = 100;
        }
    }
}