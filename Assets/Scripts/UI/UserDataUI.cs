using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserDataUI : MonoBehaviour
{
    [SerializeField] UserManager UserManager;
    [SerializeField] TextMeshProUGUI AccountId;
    [SerializeField] TextMeshProUGUI Name;
    [SerializeField] TextMeshProUGUI CurrentPassword;
    [SerializeField] TextMeshProUGUI Role;
    [SerializeField] TMP_InputField _passwordField;
    [SerializeField] Button _passwordButton;

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
        var canChangePassword = UserManager.UpdatePassword(_passwordField.text);
        
        if (canChangePassword)
        {
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        var user = UserManager.CurrentUserData;

        AccountId.text = "Account ID: " + user.Id;
        Name.text = "Username: " + user.Username;
        CurrentPassword.text = "Current password: " + user.Password;
        Role.text = "Role: " + user.Role;
        _passwordField.text = "";
    }
}