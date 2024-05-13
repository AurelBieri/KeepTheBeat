namespace Keep_The_Beat.Classes
{
    public class User : Person
    {
        public string _username { get; set; }
        public string _password { get; set; }
        public string _email { get; set; }

        public User(string username, string password, string email, string firstname, string lastname) : base(firstname, lastname)
        {
            _username = username;
            _password = password;
            _email = email;
        }
    }
}
