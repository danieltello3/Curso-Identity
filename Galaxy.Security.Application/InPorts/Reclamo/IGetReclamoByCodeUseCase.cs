using Galaxy.Security.Application.Dto.Reclamo;

namespace Galaxy.Security.Application.InPorts.Reclamo
{
    public interface IGetReclamoByCodeUseCase
    {
        Task<GetReclamoResponse> ExecuteAsync(string code);
    }
}