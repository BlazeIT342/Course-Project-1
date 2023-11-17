using TMPro;
using UnityEngine;

namespace Project.UI.Menu
{   /// <summary>
    /// Represents a user interface element for displaying messages to the player.
    /// </summary>
    public class MessageUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMessage;
        [SerializeField] private GameObject _body;

        private void OnEnable()
        {
            CloseTab();
        }

        public void ShowMessage(string text)
        {
            _textMessage.text = text;
            _body.SetActive(true);
        }

        public void CloseTab()
        {
            _body.SetActive(false);
        }
    }
}