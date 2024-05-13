namespace Keep_The_Beat.Classes
{
    public abstract class Person
    {
        public string _firstname { get; set; }
        public string _lastname { get; set; }

        public Person(string firstname, string lastname)
        {
            _firstname = firstname;
            _lastname = lastname;
        }
    }
}
