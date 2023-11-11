using Project.Managing;
using System.Collections.Generic;
using UnityEngine;

namespace Project.UI.Menu
{
    public class RecordMenuUI : MonoBehaviour
    {
        private UserManager _userManager;
        [SerializeField] private RecordRowItemUI _recordRowItemUI;
        [SerializeField] private Transform _contentRoot;

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
            _userManager = FindObjectOfType<GameManager>().UserManager;
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
            Users = _userManager.GetAllUsersSortedByParameter(ColumnType.Record);
        }

        public void SortByID()
        {
            Users = _userManager.GetAllUsersSortedByParameter(ColumnType.Id);
        }

        public void SortByUsername()
        {
            Users = _userManager.GetAllUsersSortedByParameter(ColumnType.Username);
        }
    }
}