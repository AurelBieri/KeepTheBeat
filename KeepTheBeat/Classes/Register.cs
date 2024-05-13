namespace Keep_The_Beat.Classes
{
    public class Register
    {
        public string _Rusername { get; set; }
        public string _Rpassword { get; set; }
        public string _Remail { get; set; }

        public string _Rname { get; set; }
        public DateTime _Rbirthday { get; set; }
        public Register(string Rusername, string Rpassword, string Remail, string Rname, DateTime Rbirthday)
        {

            _Remail = Rusername;
            _Rusername = Rpassword;
            _Rpassword = Remail;
            _Rname = Rname;
            _Rbirthday = Rbirthday;
        }
    }
}
