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
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _usernameField;
        [SerializeField] private TMP_InputField _passwordField;
        [SerializeField] private Toggle _administratorToggle;
        [SerializeField] private GameObject _registrationMenu;
        [SerializeField] private GameObject _mainMenu;
        [SerializeField] private GameObject _accountData;
        [SerializeField] private GameObject _recordTable;

        private DatabaseController _databaseController;

        private void OnEnable()
        {
            _databaseController = GameManager.Instance.DatabaseController;

            if (!string.IsNullOrEmpty(_databaseController.CurrentUserData.Username))
            {
                OnLoadMenuToggle();
            }
        }

        public void OnRegisterButtonClick()
        {
            var registrationSuccess = _databaseController.TryRegisterUser(_usernameField.text, _passwordField.text, _administratorToggle.isOn);

            if (registrationSuccess)
            {
                OnLoadMenuToggle();
            }
        }

        public void OnLoginButtonClick()
        {
            var loginSuccess = _databaseController.TryLoginUser(_usernameField.text, _passwordField.text);

            if (loginSuccess)
            {
                OnLoadMenuToggle();
            }
        }

        public void OnLoadMenuToggle()
        {
            _registrationMenu.SetActive(!_registrationMenu.activeSelf);
            _mainMenu.SetActive(!_mainMenu.activeSelf);
        }

        public void OnPersonalPageToggle()
        {
            _mainMenu.SetActive(!_mainMenu.activeSelf);
            _accountData.SetActive(!_accountData.activeSelf);
        }

        public void OnRecordPageToggle()
        {
            _mainMenu.SetActive(!_mainMenu.activeSelf);
            _recordTable.SetActive(!_recordTable.activeSelf);
        }

        public void OnResetData()
        {
            _databaseController.ClearUserData();
        }

        public void OnPlayButtonClick()
        {
            SceneManager.LoadSceneAsync("GameScene");
        }

        public void SignOut()
        {
            _accountData.SetActive(!_accountData.activeSelf);
            _registrationMenu.SetActive(!_registrationMenu.activeSelf);
            _databaseController.Logout();
        }

        public void Quit()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
        }
    }
}