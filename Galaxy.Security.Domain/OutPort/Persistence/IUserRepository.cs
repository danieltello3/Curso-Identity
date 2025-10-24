using Galaxy.Security.Domain.Dpo;
using Galaxy.Security.Domain.Entities;

namespace Galaxy.Security.Domain.OutPort.Persistence
{
    public interface IUserRepository
    {
        Task<OperationResult> CreateUserAsync(User user, string role);
        Task<User?> FindByUserNameAsync(string userName);
        Task<bool> CheckPasswordAsync(User user, string password);
    }
}
