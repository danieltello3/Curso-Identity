using Galaxy.Security.Application.Dto;
using Galaxy.Security.Application.Dto.Reclamo;

namespace Galaxy.Security.Application.InPorts.Reclamo
{
    public interface ICreateReclamoUseCase
    {
        Task<IdentityResponse> ExecuteAsync(CreateReclamoRequest request);
    }
}