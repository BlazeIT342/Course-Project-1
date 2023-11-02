using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserDataUI : MonoBehaviour
{
    [SerializeField] GameManager GameManager;
    [SerializeField] TextMeshProUGUI AccountId;
    [SerializeField] TextMeshProUGUI Name;
    [SerializeField] TextMeshProUGUI CurrentPassword;
    [SerializeField] TextMeshProUGUI Role;
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
        GameManager.UserManager.UpdatePassword(GameManager.UserManager.CurrentUserId, _passwordField.text);
        //GameManager.UserManager.UpdatePassword(GameManager.UserManager.CurrentUserId, _passwordField.text);
        UpdateUI();
    }

    private void UpdateUI()
    {
        AccountId.text = "Account ID: " + GameManager.UserManager.CurrentUserId.ToString();
        Name.text = "Name: " + GameManager.UserManager.Username;
        CurrentPassword.text = "Current password: " + GameManager.UserManager.Password;
        Role.text = "Role: " + GameManager.UserManager.Role;
    }
}