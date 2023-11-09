using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

namespace Project.UI.Menu
{
    public class RegexInputField : MonoBehaviour
    {
        [SerializeField] private string _regexPattern;
        private TMP_InputField _inputField;

        void Start()
        {
            _inputField = GetComponent<TMP_InputField>();
            _inputField.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(string newValue)
        {
            string cleanedInput = Regex.Replace(newValue, _regexPattern, "");
            if (cleanedInput.Length > 20)
            {
                cleanedInput = cleanedInput.Substring(0, 20);
            }

            if (cleanedInput != newValue)
            {
                _inputField.text = cleanedInput;
            }
        }
    }
}