using UnityEngine;

namespace Project.UI.Menu
{    /// <summary>
     /// A component that facilitates the activation and deactivation of specified GameObjects to switch UI elements.
     /// </summary>
    public class SwitchUI : MonoBehaviour
    {
        [SerializeField] private GameObject _activate = null;
        [SerializeField] private GameObject _deactivate = null;

        public void Switch()
        {
            _activate.SetActive(true);
            _deactivate.SetActive(false);
        }
    }
}