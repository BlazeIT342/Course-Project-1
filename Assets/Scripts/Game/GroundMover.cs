using System.Collections;
using System.Collections.Generic;
using Project.Managing;
using UnityEngine;

namespace Project.Game
{
    /// <summary>
    /// Controls the respawn and generation of ground elements, responding to collision events and ensuring a cooldown period.
    /// </summary>
    public class GroundMover : MonoBehaviour
    {
        private const int DistanceToNextGround = 30; // Distance to spawn the next ground element.

        [SerializeField] private List<GameObject> _groundPrefabs = new(); // List of ground prefabs to be spawned.
        [SerializeField] private List<GameObject> _grounds = new();      // List of instantiated ground elements.

        private bool _isReadyToRespawn = true; // Indicates whether it's ready to respawn a new ground element.
        private GameEventManager _gameEventManager;

        private void OnEnable()
        {
            _gameEventManager = GameEventManager.Instance;

            _gameEventManager.OnCollisionWall.AddListener(OnCollisionWall);
        }

        private void OnDisable()
        {
            _gameEventManager.OnCollisionWall.RemoveListener(OnCollisionWall);
        }

        /// <summary>
        /// Initiates the respawn of a ground element, subject to cooldown and game state conditions.
        /// </summary>
        /// <param name="isGameRunning">Indicates whether the game is currently running.</param>
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

        /// <summary>
        /// Handles the respawn of a ground element upon colliding with a wall during gameplay.
        /// </summary>
        /// <param name="isGameRunning">Indicates whether the game is currently running.</param>
        private void OnCollisionWall(bool isGameRunning)
        {
            RespawnGround(isGameRunning);
        }

        /// <summary>
        /// Implements a cooldown period for ground respawns to prevent rapid generation.
        /// </summary>
        /// <returns>Coroutine enumerator.</returns>
        private IEnumerator RespawnCooldown()
        {
            _isReadyToRespawn = false;
            yield return new WaitForSecondsRealtime(1);
            _isReadyToRespawn = true;
        }
    }
}