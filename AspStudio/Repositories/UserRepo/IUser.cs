using AspStudio.Models.DTOs.UserD;

namespace AspStudio.Repositories.UserRepo
{
    public interface IUser
    {
        Task RegisterUser(UsersDTO model);
    }
}
