using System.Collections.Generic;
using UnityEngine;

public class RecordMenuUI : MonoBehaviour
{
    [SerializeField] UserManager UserManager;
    [SerializeField] RecordRowItemUI recordRowItemUI;
    [SerializeField] Transform contentRoot;

    private List<UserData> _users = new();
    private List<UserData> Users
    {
        get => _users;
        set
        {
            if (_users != value)
            {
                _users = value;
                BuildRecordTable();
            }
        }
    }

    private void OnEnable()
    {
        SortByRecord();
    }

    private void BuildRecordTable()
    {
        foreach (Transform item in contentRoot)
        {
            Destroy(item.gameObject);
        }

        foreach (var user in Users)
        {
            RecordRowItemUI uiInstance = Instantiate(recordRowItemUI, contentRoot);
            uiInstance.Initialize(user.Id, user.Username, user.Record);
        }
    }

    public void SortByRecord()
    {
        Users = UserManager.GetAllUsersSortedByParameter(ColumnType.Record);
    }

    public void SortByID()
    {
        Users = UserManager.GetAllUsersSortedByParameter(ColumnType.Id);
    }

    public void SortByUsername()
    {
        Users = UserManager.GetAllUsersSortedByParameter(ColumnType.Username);
    }
}