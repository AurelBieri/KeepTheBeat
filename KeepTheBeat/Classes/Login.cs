namespace Keep_The_Beat.Classes
{
    public class Login
    {
        public string _Lusername { get; set; }
        public string _Lpassword { get; set; }
        public string _Lemail { get; set; }
        public Login(string Lusername, string Lpassword, string Lemail)
        {

            _Lemail = Lusername;
            _Lusername = Lpassword;
            _Lpassword = Lemail;
        }


    }
}
