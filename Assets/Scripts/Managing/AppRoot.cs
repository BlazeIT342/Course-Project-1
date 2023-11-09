using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Managing
{
    public class AppRoot : MonoBehaviour
    {
        private void OnEnable()
        {
            SceneManager.LoadSceneAsync("MenuScene");
        }
    }
}