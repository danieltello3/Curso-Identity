using Galaxy.Security.Domain.Dpo;
using Galaxy.Security.Domain.Entities;

namespace Galaxy.Security.Domain.OutPort.Persistence
{
    public interface IReclamoRepository
    {
        Task<OperationResult> CreateAsync(Reclamo reclamo);
        Task<Reclamo?> FindByCodeAsync(string codigo);
        Task<int> CountAsync();
    }
}