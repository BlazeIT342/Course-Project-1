using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Project.Managing
{
    public class MenuManager : MonoBehaviour
    {
        private UserManager _userManager;
        [SerializeField] private TMP_InputField _usernameField;
        [SerializeField] private TMP_InputField _passwordField;
        [SerializeField] private Toggle _administratorToggle;
        [SerializeField] private GameObject _registrationMenu;
        [SerializeField] private GameObject _mainMenu;
        [SerializeField] private GameObject _accountData;
        [SerializeField] private GameObject _recordTable;

        private void OnEnable()
        {
            _userManager = FindObjectOfType<GameManager>().UserManager;

            if (!string.IsNullOrEmpty(_userManager.CurrentUserData.Username))
            {
                OnLoadMenuToggle();
            }
        }

        public void OnRegisterButtonClick()
        {
            var registrationSuccess = _userManager.TryRegisterUser(_usernameField.text, _passwordField.text, _administratorToggle.isOn);

            if (registrationSuccess)
            {
                OnLoadMenuToggle();
            }
        }

        public void OnLoginButtonClick()
        {
            var loginSuccess = _userManager.TryLoginUser(_usernameField.text, _passwordField.text);

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
            _userManager.ClearUserData();
        }

        public void OnPlayButtonClick()
        {
            SceneManager.LoadSceneAsync("GameScene");
        }

        public void SignOut()
        {
            _accountData.SetActive(!_accountData.activeSelf);
            _registrationMenu.SetActive(!_registrationMenu.activeSelf);
            _userManager.Logout();
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