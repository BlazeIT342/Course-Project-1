using Project.Managing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Menu
{    /// <summary>
     /// Represents a UI item for displaying and interacting with user information in the admin menu.
     /// </summary>
    public class AdminRowItemUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _id;
        [SerializeField] private TextMeshProUGUI _nickname;
        [SerializeField] private TextMeshProUGUI _password;
        [SerializeField] private TextMeshProUGUI _role;
        [SerializeField] private Button _deleteButton;

        public void Initialize(int id, string nickname, string password, string role)
        {
            _id.text = $"{id}";
            _nickname.text = nickname;
            _password.text = $"{password}";
            _role.text = $"{role}";

            _deleteButton.onClick.AddListener(() =>
            {
                DatabaseManager.Instance.DatabaseController.DeleteUserByUsername(nickname);
                Destroy(gameObject);
            });
        }
    }
}