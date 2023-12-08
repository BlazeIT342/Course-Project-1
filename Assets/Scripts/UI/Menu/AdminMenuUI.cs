using Project.Database;
using Project.Managing;
using System.Collections.Generic;
using UnityEngine;

namespace Project.UI.Menu
{   /// <summary>
    /// Manages the administrative menu UI, allowing administrators to view and sort user data.
    /// </summary>
    public class AdminMenuUI : MonoBehaviour
    {
        [SerializeField] private AdminRowItemUI _adminRowItemUI;
        [SerializeField] private Transform _contentRoot;

        private DatabaseManager _databaseManager;

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
        private bool _passwordSortDesc;
        private bool _roleSortDesc;

        private void OnEnable()
        {
            _databaseManager = DatabaseManager.Instance;

            _idSortDesc = false;
            _usernameSortDesc = false;
            _passwordSortDesc = false;
            _roleSortDesc = false;

            SortByID();
        }

        private void BuildRecordTable()
        {
            foreach (Transform item in _contentRoot)
            {
                Destroy(item.gameObject);
            }

            foreach (var user in Users)
            {
                if (user.Id == _databaseManager.DatabaseController.CurrentUserData.Id)
                    continue;

                AdminRowItemUI uiInstance = Instantiate(_adminRowItemUI, _contentRoot);
                uiInstance.Initialize(user.Id, user.Username, user.Password, user.Role);
            }
        }

        public void SortByID()
        {
            Users = _databaseManager.DatabaseController.GetAllUsersSortedByParameter("Id", _idSortDesc);

            _idSortDesc = !_idSortDesc;
        }

        public void SortByUsername()
        {
            Users = _databaseManager.DatabaseController.GetAllUsersSortedByParameter("Username", _usernameSortDesc);

            _usernameSortDesc = !_usernameSortDesc;
        }

        public void SortByPassword()
        {
            Users = _databaseManager.DatabaseController.GetAllUsersSortedByParameter("Password", _passwordSortDesc);

            _passwordSortDesc = !_passwordSortDesc;
        }

        public void SortByRole()
        {
            Users = _databaseManager.DatabaseController.GetAllUsersSortedByParameter("Role", _roleSortDesc);

            _roleSortDesc = !_roleSortDesc;
        }
    }
}