using Keep_The_Beat.Classes;

namespace KeepTheBeat.Interfaces
{
    public interface IUserService
    {
        Task AddUser(User user);
        Task<List<User>> GetUser();
        Task<User> Login(string username, string password);
        Task<bool> IsUsernameTaken(string username);
        Task<bool> IsEmailTaken(string email);

    }
}
