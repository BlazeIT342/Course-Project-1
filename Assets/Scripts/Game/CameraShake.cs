using Cinemachine;
using Project.Managing;
using UnityEngine;

namespace Project.Game
{
    public class CameraShake : MonoBehaviour
    {
        private const float ShakeAmplitude = 2f;
        private const float ShakeFrequency = 3.0f;
        private const float Duration = 0.2f;

        private float _shakeElapsedTime = 0f;
        private CinemachineVirtualCamera _virtualCamera;

        private void OnEnable()
        {
            GameEventManager.Instance.OnAddNewCube.AddListener(OnAddNewCube);
            GameEventManager.Instance.OnCollisionWall.AddListener(OnCollisionWall);
        }

        private void OnDisable()
        {
            GameEventManager.Instance.OnAddNewCube.RemoveListener(OnAddNewCube);
            GameEventManager.Instance.OnCollisionWall.RemoveListener(OnCollisionWall);
        }

        private void Start()
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        }

        private void Update()
        {
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

        public void ShakeCamera(float duration)
        {
            CinemachineBasicMultiChannelPerlin noise = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            noise.m_AmplitudeGain = ShakeAmplitude;
            noise.m_FrequencyGain = ShakeFrequency;
            _shakeElapsedTime = duration;
        }

        private void OnAddNewCube(bool isGameRunning)
        {
            ShakeCamera(Duration);
        }

        private void OnCollisionWall(bool isGameRunning)
        {
            ShakeCamera(Duration);
        }
    }
}