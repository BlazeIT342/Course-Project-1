using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    [SerializeField] public UserManager UserManager;
    [SerializeField] GameObject RegistrationMenu;
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject AccountData;

    public void OnRegisterButtonClick()
    {
        bool registrationSuccess = UserManager.RegisterUser();
    }

    public void OnLoginButtonClick()
    {
        bool loginSuccess = UserManager.LoginUser();

        if (loginSuccess)
        {
            RegistrationMenu.SetActive(false);
            MainMenu.SetActive(true);
        }
    }

    public void OnPersonalPageButtonClick()
    {
        MainMenu.SetActive(false);
        AccountData.SetActive(true);
    }

    public void OnPersonalPageButtonClickUndo()
    {
        MainMenu.SetActive(true);
        AccountData.SetActive(false);
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