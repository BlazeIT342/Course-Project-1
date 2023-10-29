using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class RegexInputField : MonoBehaviour
{
    [SerializeField] string regexPattern;
    private TMP_InputField inputField;

    void Start()
    {
        inputField = GetComponent<TMP_InputField>();
        inputField.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(string newValue)
    {
        string cleanedInput = Regex.Replace(newValue, regexPattern, "");
        if (cleanedInput.Length > 20)
        {
            cleanedInput = cleanedInput.Substring(0, 20);
        }

        if (cleanedInput != newValue)
        {
            inputField.text = cleanedInput;
        }
    }
}