using Galaxy.Security.Domain.Dpo;
using Galaxy.Security.Domain.Entities;
using Galaxy.Security.Domain.OutPort.Persistence;
using Galaxy.Security.Infraestructure.Configurations.Context;
using Microsoft.EntityFrameworkCore;

namespace Galaxy.Security.Infraestructure.Adapters.Persistence
{
    public class ReclamoRepository : IReclamoRepository
    {
        private readonly IdentityDbContext _context;
        public ReclamoRepository(IdentityDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult> CreateAsync(Reclamo reclamo)
        {
            await _context.AddAsync(reclamo);
            await _context.SaveChangesAsync();
            return OperationResult.Ok();
        }

        public async Task<Reclamo?> FindByCodeAsync(string codigo)
        {
            var reclamo = await _context.Reclamos.SingleOrDefaultAsync(x => x.Codigo.Equals(codigo));
            return reclamo;
        }

        public async Task<int> CountAsync()
        {
            var cantidad = await _context.Reclamos.CountAsync();
            return cantidad;
        }

    }
}