using Project.Managing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Game
{
    /// <summary>
    /// Manages the spawning, removal, and visual effects of cubes during gameplay, interacting with events from the game manager.
    /// </summary>
    public class CubeHolder : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _cubeEffect;          // Reference to the particle system for cube effects.
        [SerializeField] private Cube _cubePrefab;                    // Reference to the cube prefab to be instantiated.
        [SerializeField] private GameObject _collectText;             // Reference to the collect text object.
        [SerializeField] private Transform _cubeHolderTransform;       // Transform representing the position of the cube holder.
        [SerializeField] private Transform _collectTextTransform;      // Transform representing the position of the collect text.
        [SerializeField] private Transform _cubeRemoverTransform;      // Transform representing the position where cubes are removed.
        [SerializeField] private List<Cube> _cubeList = new List<Cube>(); // List to keep track of instantiated cubes.

        private bool _isGameRunning = true; // Indicates whether the game is currently running.

        private void OnEnable()
        {
            GameEventManager.Instance.OnAddNewCube.AddListener(OnAddNewCube);
            GameEventManager.Instance.OnGameEnd.AddListener(OnGameEnd);
        }

        private void OnDisable()
        {
            GameEventManager.Instance.OnAddNewCube.RemoveListener(OnAddNewCube);
            GameEventManager.Instance.OnGameEnd.RemoveListener(OnGameEnd);
        }

        /// <summary>
        /// Coroutine to remove a cube after a specified delay and update the cube list.
        /// </summary>
        /// <param name="cube">The cube to be removed.</param>
        /// <returns>Coroutine enumerator.</returns>
        public IEnumerator RemoveCube(Cube cube)
        {
            cube.transform.SetParent(_cubeRemoverTransform);
            yield return new WaitForSecondsRealtime(2);
            if (!_isGameRunning) yield break;
            _cubeList.Remove(cube);
            Destroy(cube.gameObject, 2f);
        }

        /// <summary>
        /// Handles actions to be performed when the game ends.
        /// </summary>
        /// <param name="isGameRunning">Indicates whether the game is currently running.</param>
        private void OnGameEnd(bool isGameRunning)
        {
            _isGameRunning = isGameRunning;
        }

        /// <summary>
        /// Handles actions to be performed when a new cube is added during gameplay.
        /// </summary>
        /// <param name="isGameRunning">Indicates whether the game is currently running.</param>
        private void OnAddNewCube(bool isGameRunning)
        {
            transform.position += Vector3.up;
            Cube cubeInstance = Instantiate(_cubePrefab, _cubeHolderTransform);
            cubeInstance.transform.position = new Vector3(_cubeList[0].transform.position.x, _cubeList[_cubeList.Count - 1].transform.position.y - 1, _cubeList[0].transform.position.z);
            GameObject textInstance = Instantiate(_collectText, _collectTextTransform);
            _cubeEffect.Play();
            GetComponentInChildren<Animator>().SetTrigger("Jump");
            Destroy(textInstance, 5f);
            _cubeList.Add(cubeInstance);
        }
    }
}