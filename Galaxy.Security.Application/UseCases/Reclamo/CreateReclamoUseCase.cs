using Galaxy.Security.Application.Dto;
using Galaxy.Security.Application.Dto.Reclamo;
using Galaxy.Security.Application.InPorts.Reclamo;
using Galaxy.Security.Domain.OutPort.Persistence;

namespace Galaxy.Security.Application.UseCases.Reclamo
{
    public class CreateReclamoUseCase : ICreateReclamoUseCase
    {

        private readonly IReclamoRepository _reclamoRepository;
        public CreateReclamoUseCase(IReclamoRepository reclamoRepository)
        {
            _reclamoRepository = reclamoRepository;
        }

        public async Task<IdentityResponse> ExecuteAsync(CreateReclamoRequest request)
        {
            var codigo = await ObtenerCodigo();

            var user = Domain.Entities.Reclamo.Create(codigo, request.Descripcion, request.Fecha);
            var result = await _reclamoRepository.CreateAsync(user);

            return new IdentityResponse
            {
                Data = user.Codigo,
                Success = result.Success,
                Errors = result.Errors
            };
        }

        private async Task<string> ObtenerCodigo()
        {

            int cantidadReclamo = await _reclamoRepository.CountAsync();
            return (cantidadReclamo + 1).ToString("D5");

        }

    }
}
