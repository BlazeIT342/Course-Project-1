using Project.Database;
using Project.Managing;
using System.Collections.Generic;
using UnityEngine;

namespace Project.UI.Menu
{    /// <summary>
     /// Represents the UI for displaying and sorting user records, including sorting by ID, username, and highest score.
     /// </summary>
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

        private bool _idSortDesc;
        private bool _usernameSortDesc;
        private bool _recordSortDesc;

        private void OnEnable()
        {
            _databaseController = DatabaseManager.Instance.DatabaseController;

            _idSortDesc = false;
            _usernameSortDesc = false;
            _recordSortDesc = true;

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
            Users = _databaseController.GetAllUsersSortedByParameter("Record", _recordSortDesc);

            _recordSortDesc = !_recordSortDesc;
        }

        public void SortByID()
        {
            Users = _databaseController.GetAllUsersSortedByParameter("Id", _idSortDesc);

            _idSortDesc = !_idSortDesc;
        }

        public void SortByUsername()
        {
            Users = _databaseController.GetAllUsersSortedByParameter("Username", _usernameSortDesc);

            _usernameSortDesc = !_usernameSortDesc;
        }
    }
}