namespace Keep_The_Beat.Classes
{
    public class User : Person
    {
        public string _username { get; set; }
        public string _password { get; set; }
        public string _email { get; set; }
        public DateTime _birthday { get; set; }

        public User(string username, string password, string email, string firstname, string lastname, DateTime birthday) : base(firstname, lastname)
        {
            _username = username;
            _password = password;
            _email = email;
            _birthday = birthday;
        }
    }
}
