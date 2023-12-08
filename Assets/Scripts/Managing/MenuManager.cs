using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Project.Database;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Project.Managing
{
    /// <summary>
    /// Manager class for handling user interaction with menus, authentication, and navigation within the application.
    /// </summary>
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _usernameField;
        [SerializeField] private TMP_InputField _passwordField;
        [SerializeField] private Toggle _administratorToggle;
        [SerializeField] private GameObject _registrationMenu;
        [SerializeField] private GameObject _mainMenu;
        [SerializeField] private GameObject _accountData;
        [SerializeField] private GameObject _recordTable;
        [SerializeField] private GameObject _aboutMenu;
        [SerializeField] private GameObject _adminMenu;
        [SerializeField] private Button _adminButton;

        private DatabaseManager _databaseManager;

        private void OnEnable()
        {
            _databaseManager = DatabaseManager.Instance;

            if (!string.IsNullOrEmpty(_databaseManager.DatabaseController.CurrentUserData.Username))
            {
                OnLoadMenuToggle();
            }
        }

        public void OnRegisterButtonClick()
        {
            var registrationSuccess = _databaseManager.DatabaseController.TryRegisterUser(_usernameField.text, _passwordField.text, _administratorToggle.isOn);

            if (registrationSuccess)
            {
                OnLoadMenuToggle();
            }
        }

        public void OnLoginButtonClick()
        {
            var loginSuccess = _databaseManager.DatabaseController.TryLoginUser(_usernameField.text, _passwordField.text);

            if (loginSuccess)
            {
                OnLoadMenuToggle();
            }
        }

        public void OnLoadMenuToggle()
        {
            _registrationMenu.SetActive(!_registrationMenu.activeSelf);
            ToggleMainMenu();

            _adminButton.gameObject.SetActive(_databaseManager.DatabaseController.CurrentUserData.Role == "Administrator");
        }

        public void OnPersonalPageToggle()
        {
            ToggleMainMenu();
            _accountData.SetActive(!_accountData.activeSelf);
        }

        public void OnRecordMenuToggle()
        {
            ToggleMainMenu();
            _recordTable.SetActive(!_recordTable.activeSelf);
        }

        public void OnAboutMenuToggle()
        {
            ToggleMainMenu();
            _aboutMenu.SetActive(!_aboutMenu.activeSelf);
        }

        public void OnAdminMenuToggle()
        {
            ToggleMainMenu();
            _adminMenu.SetActive(!_adminMenu.activeSelf);
        }

        public void OnResetData()
        {
            _databaseManager.DatabaseController.ClearUserData();
        }

        public void OnPlayButtonClick()
        {
            SceneManager.LoadSceneAsync("GameScene");
        }

        public void SignOut()
        {
            _accountData.SetActive(!_accountData.activeSelf);
            _registrationMenu.SetActive(!_registrationMenu.activeSelf);
            _databaseManager.DatabaseController.ClearCurrentUserData();
        }

        public void Quit()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
        }

        private void ToggleMainMenu()
        {
            _mainMenu.SetActive(!_mainMenu.activeSelf);
        }
    }
}