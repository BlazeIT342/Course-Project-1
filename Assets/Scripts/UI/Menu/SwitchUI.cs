using UnityEngine;

namespace Project.UI.Menu
{
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