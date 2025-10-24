using Galaxy.Security.Application.Dto;
using Galaxy.Security.Application.Dto.Reclamo;
using Galaxy.Security.Application.InPorts.Reclamo;
using Galaxy.Security.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Galaxy.Security.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReclamoController : ControllerBase
    {

        private readonly ICreateReclamoUseCase _createReclamoUseCase;
        private readonly IGetReclamoByCodeUseCase _getReclamoByCodeUseCase;
        public ReclamoController(ICreateReclamoUseCase createReclamoUseCase, IGetReclamoByCodeUseCase getReclamoByCodeUseCase)
        {
            _createReclamoUseCase = createReclamoUseCase;
            _getReclamoByCodeUseCase = getReclamoByCodeUseCase;
        }

        [HttpPost("Create")]
        [Authorize(Roles = RolesConstants.ManagerRole)]
        public async Task<IActionResult> Create([FromBody] CreateReclamoRequest request)
        {
            var result = await _createReclamoUseCase.ExecuteAsync(request);
            return Ok(BaseResponse<IdentityResponse>.Success(result));
        }

        [HttpGet("Get/{code}")]
        [Authorize(Roles = RolesConstants.CustomerRole)]
        public async Task<IActionResult> Get(string code)
        {
            var result = await _getReclamoByCodeUseCase.ExecuteAsync(code);
            return Ok(BaseResponse<object>.Success(result));
        }

    }
}
