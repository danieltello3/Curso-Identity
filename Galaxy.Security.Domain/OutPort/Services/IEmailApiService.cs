using Galaxy.Security.Domain.Dpo;

namespace Galaxy.Security.Domain.OutPort.Services
{
    public interface IEmailApiService
    {
        Task SendEmailAsync(SendEmailDpo request);
    }
}
