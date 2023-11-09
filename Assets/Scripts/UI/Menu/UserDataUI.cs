using Project.Managing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Menu
{
    public class UserDataUI : MonoBehaviour
    {
        [SerializeField] private UserManager _userManager;
        [SerializeField] private TextMeshProUGUI _accountId;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _currentPassword;
        [SerializeField] private TextMeshProUGUI _role;
        [SerializeField] private TMP_InputField _passwordField;
        [SerializeField] private Button _passwordButton;

        private void OnEnable()
        {
            _passwordButton.onClick.AddListener(ChangePassword);
            UpdateUI();
        }

        private void OnDisable()
        {
            _passwordButton.onClick.RemoveListener(ChangePassword);
            UpdateUI();
        }
        private void ChangePassword()
        {
            var canChangePassword = _userManager.UpdatePassword(_passwordField.text);

            if (canChangePassword)
            {
                UpdateUI();
            }
        }

        private void UpdateUI()
        {
            var user = _userManager.CurrentUserData;

            _accountId.text = "Account ID: " + user.Id;
            _name.text = "Username: " + user.Username;
            _currentPassword.text = "Current password: " + user.Password;
            _role.text = "Role: " + user.Role;
            _passwordField.text = "";
        }
    }
}