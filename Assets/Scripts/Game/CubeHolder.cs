using Project.Managing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Game
{
    public class CubeHolder : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _cubeEffect;
        [SerializeField] private Cube _cubePrefab;
        [SerializeField] private GameObject _collectText;
        [SerializeField] private Transform _cubeHolderTransform;
        [SerializeField] private Transform _collectTextTransform;
        [SerializeField] private Transform _cubeRemoverTransform;
        [SerializeField] private List<Cube> _cubeList = new();

        private bool _isGameRunning = true;

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

        public IEnumerator RemoveCube(Cube cube)
        {
            cube.transform.SetParent(_cubeRemoverTransform);
            yield return new WaitForSecondsRealtime(2);
            if (!_isGameRunning) yield break;
            _cubeList.Remove(cube);
            Destroy(cube.gameObject, 2f);
        }

        private void OnGameEnd(bool isGameRunning)
        {
            _isGameRunning = isGameRunning;
        }

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