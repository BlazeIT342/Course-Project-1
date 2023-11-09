using System.Collections;
using System.Collections.Generic;
using Project.Managing;
using UnityEngine;

namespace Project.Game
{
    public class GroundMover : MonoBehaviour
    {
        private const int DistanceToNextGround = 30;

        [SerializeField] private List<GameObject> _groundPrefabs = new();
        [SerializeField] private List<GameObject> _grounds = new();

        private bool _isReadyToRespawn = true;

        private void OnEnable()
        {
            GameEventManager.Instance.OnCollisionWall.AddListener(OnCollisionWall);
        }

        private void OnDisable()
        {
            GameEventManager.Instance.OnCollisionWall.RemoveListener(OnCollisionWall);
        }

        public void RespawnGround(bool isGameRunning)
        {
            if (!isGameRunning || !_isReadyToRespawn) return;
            StartCoroutine(RespawnCooldown());
            Destroy(_grounds[0]);
            _grounds.RemoveAt(0);
            GameObject newGround = Instantiate(_groundPrefabs[Random.Range(0, _groundPrefabs.Count)], transform.position, Quaternion.identity, transform);
            newGround.transform.position = new Vector3(0, _grounds[^1].transform.position.y, _grounds[^1].transform.position.z + DistanceToNextGround);
            _grounds.Add(newGround);
        }

        private void OnCollisionWall(bool isGameRunning)
        {
            RespawnGround(isGameRunning);
        }

        private IEnumerator RespawnCooldown()
        {
            _isReadyToRespawn = false;
            yield return new WaitForSecondsRealtime(1);
            _isReadyToRespawn = true;
        }
    }
}