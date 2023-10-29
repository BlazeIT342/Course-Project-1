using UnityEngine;

namespace Course.UI
{
    public class SwitchUI : MonoBehaviour
    {
        [SerializeField] GameObject activate = null;
        [SerializeField] GameObject deactivate = null;

        public void Switch()
        {
            activate.SetActive(true);
            deactivate.SetActive(false);
        }
    }
}