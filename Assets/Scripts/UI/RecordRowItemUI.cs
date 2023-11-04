using TMPro;
using UnityEngine;

public class RecordRowItemUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI id;
    [SerializeField] TextMeshProUGUI nickname;
    [SerializeField] TextMeshProUGUI record;

    public void Initialize(int id, string nickname, int record)
    {
        this.id.text = $"{id}";
        this.nickname.text = nickname;
        this.record.text = $"{record}";
    }
}