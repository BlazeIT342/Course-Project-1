using Cinemachine;
using Project.Managing;
using UnityEngine;

namespace Project.Game
{
    /// <summary>
    /// Controls camera shake effects triggered by specific game events, such as adding a new cube or colliding with a wall.
    /// </summary>
    public class CameraShake : MonoBehaviour
    {
        private const float ShakeAmplitude = 2f;     // The amplitude of the camera shake.
        private const float ShakeFrequency = 3.0f;   // The frequency of the camera shake.
        private const float Duration = 0.2f;          // The duration of the camera shake.

        private float _shakeElapsedTime = 0f;        // The elapsed time for the camera shake effect.
        private CinemachineVirtualCamera _virtualCamera; // Reference to the Cinemachine virtual camera.

        private GameEventManager _gameEventManager;

        private void OnEnable()
        {
            _gameEventManager = GameEventManager.Instance;

            _gameEventManager.OnAddNewCube.AddListener(OnAddNewCube);
            _gameEventManager.OnCollisionWall.AddListener(OnCollisionWall);
        }

        private void OnDisable()
        {
            _gameEventManager.OnAddNewCube.RemoveListener(OnAddNewCube);
            _gameEventManager.OnCollisionWall.RemoveListener(OnCollisionWall);
        }

        private void Start()
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>(); // Gets the Cinemachine virtual camera component.
        }

        private void Update()
        {
            // Updates the camera shake effect based on the elapsed time.
            if (_shakeElapsedTime > 0)
            {
                _shakeElapsedTime -= Time.deltaTime;
                if (_shakeElapsedTime <= 0f)
                {
                    CinemachineBasicMultiChannelPerlin noise = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                    noise.m_AmplitudeGain = 0f;
                    noise.m_FrequencyGain = 0f;
                }
            }
        }

        /// <summary>
        /// Triggers the camera shake effect for a specified duration.
        /// </summary>
        /// <param name="duration">The duration of the camera shake effect.</param>
        public void ShakeCamera(float duration)
        {
            CinemachineBasicMultiChannelPerlin noise = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            noise.m_AmplitudeGain = ShakeAmplitude;
            noise.m_FrequencyGain = ShakeFrequency;
            _shakeElapsedTime = duration;
        }

        /// <summary>
        /// Handles the camera shake effect when a new cube is added during gameplay.
        /// </summary>
        /// <param name="isGameRunning">Indicates whether the game is currently running.</param>
        private void OnAddNewCube(bool isGameRunning)
        {
            ShakeCamera(Duration);
        }

        /// <summary>
        /// Handles the camera shake effect when a collision with a wall occurs during gameplay.
        /// </summary>
        /// <param name="isGameRunning">Indicates whether the game is currently running.</param>
        private void OnCollisionWall(bool isGameRunning)
        {
            ShakeCamera(Duration);
        }
    }
}