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
    [SerializeField] GameObject RecordTable;

    public void OnRegisterButtonClick()
    {
        var registrationSuccess = UserManager.RegisterUser();
    }

    public void OnLoginButtonClick()
    {
        var loginSuccess = UserManager.LoginUser();

        if (loginSuccess)
        {
            RegistrationMenu.SetActive(!RegistrationMenu.activeSelf);
            MainMenu.SetActive(!MainMenu.activeSelf);
        }
    }

    public void OnPersonalPageToggle()
    {
        MainMenu.SetActive(!MainMenu.activeSelf);
        AccountData.SetActive(!AccountData.activeSelf);
    }

    public void OnRecordPageToggle()
    {
        MainMenu.SetActive(!MainMenu.activeSelf);
        RecordTable.SetActive(!RecordTable.activeSelf);
    }

    public void SignOut()
    {
        AccountData.SetActive(!AccountData.activeSelf);
        RegistrationMenu.SetActive(!RegistrationMenu.activeSelf);
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