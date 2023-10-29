using UnityEngine;

public class RegisterLoginUI : MonoBehaviour
{
    [SerializeField] UserManager userManager;

    public void OnRegisterButtonClick()
    {
        bool registrationSuccess = userManager.RegisterUser();
    }

    public void OnLoginButtonClick()
    {
        bool loginSuccess = userManager.LoginUser();
    }
}