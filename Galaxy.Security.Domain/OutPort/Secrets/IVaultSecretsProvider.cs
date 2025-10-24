namespace Galaxy.Security.Domain.OutPort.Secrets
{
    public interface IVaultSecretsProvider
    {
        Task<Dictionary<string, string>> GetSecretsAsync();
        Task<string?> GetSecretAsync(string key);
    }
}
