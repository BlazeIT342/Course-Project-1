using Project.Database;
using Project.Managing;
using System.Collections.Generic;
using UnityEngine;

namespace Project.UI.Menu
{
    public class RecordMenuUI : MonoBehaviour
    {
        [SerializeField] private RecordRowItemUI _recordRowItemUI;
        [SerializeField] private Transform _contentRoot;

        private DatabaseController _databaseController;

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
            _databaseController = GameManager.Instance.DatabaseController;
            SortByRecord();
        }

        private void BuildRecordTable()
        {
            foreach (Transform item in _contentRoot)
            {
                Destroy(item.gameObject);
            }

            foreach (var user in Users)
            {
                RecordRowItemUI uiInstance = Instantiate(_recordRowItemUI, _contentRoot);
                uiInstance.Initialize(user.Id, user.Username, user.Record);
            }
        }

        public void SortByRecord()
        {
            Users = _databaseController.GetAllUsersSortedByParameter("Record");
        }

        public void SortByID()
        {
            Users = _databaseController.GetAllUsersSortedByParameter("Id");
        }

        public void SortByUsername()
        {
            Users = _databaseController.GetAllUsersSortedByParameter("Username");
        }
    }
}