using Galaxy.Security.Application.Dto.Reclamo;
using Galaxy.Security.Application.InPorts.Reclamo;
using Galaxy.Security.Domain.OutPort.Persistence;

namespace Galaxy.Security.Application.UseCases.Reclamo
{
    public class GetReclamoByCodeUseCase : IGetReclamoByCodeUseCase
    {

        private readonly IReclamoRepository _reclamoRepository;
        public GetReclamoByCodeUseCase(IReclamoRepository reclamoRepository)
        {
            _reclamoRepository = reclamoRepository;
        }

        public async Task<GetReclamoResponse> ExecuteAsync(string code)
        {
            var reclamo = await _reclamoRepository.FindByCodeAsync(code);

            if (reclamo is null) { 
                throw new ApplicationException($"Reclamo with code {code} not found.");
            }

            return new GetReclamoResponse
            {
                Id = reclamo.Id,
                Codigo = reclamo.Codigo,
                Descripcion = reclamo.Descripcion,
                Fecha = reclamo.Fecha
            };
        }

    }
}