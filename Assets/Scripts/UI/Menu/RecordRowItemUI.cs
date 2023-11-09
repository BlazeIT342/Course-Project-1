using TMPro;
using UnityEngine;

namespace Project.UI.Menu
{
    public class RecordRowItemUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _id;
        [SerializeField] private TextMeshProUGUI _nickname;
        [SerializeField] private TextMeshProUGUI _record;

        public void Initialize(int id, string nickname, int record)
        {
            _id.text = $"{id}";
            _nickname.text = nickname;
            _record.text = $"{record}";
        }
    }
}