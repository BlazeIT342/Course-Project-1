namespace Project.Database
{    /// <summary>
     /// Represents a structure containing user data, including ID, username, password, role, and highest _score.
     /// </summary>
    public struct UserData
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public int Record { get; set; }
    }
}