namespace Galaxy.Security.Application.InPorts.Users
{
    public interface IRefreshTokenUseCase
    {
        Task<string> ExecuteAsync();
    }
}
