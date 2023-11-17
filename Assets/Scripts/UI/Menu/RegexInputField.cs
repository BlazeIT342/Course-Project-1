using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

namespace Project.UI.Menu
{    /// <summary>
     /// A component that enforces input validation based on a specified regular expression pattern
     /// and limits the input length in a Unity TextMeshPro input field.
     /// </summary>
    public class RegexInputField : MonoBehaviour
    {
        [SerializeField] private string _regexPattern;
        private TMP_InputField _inputField;

        void Start()
        {
            _inputField = GetComponent<TMP_InputField>();
            _inputField.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(string value)
        {
            string filteredInput = Regex.Replace(value, _regexPattern, "");
            
            if (filteredInput.Length > 20)
            {
                filteredInput = filteredInput[..20];
            }

            if (filteredInput != value)
            {
                _inputField.text = filteredInput;
            }
        }
    }
}