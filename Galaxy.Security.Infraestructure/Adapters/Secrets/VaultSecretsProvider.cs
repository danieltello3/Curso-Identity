using Galaxy.Security.Domain.OutPort.Secrets;
using VaultSharp;

namespace Galaxy.Security.Infraestructure.Adapters.Secrets
{
    public class VaultSecretsProvider : IVaultSecretsProvider
    {
        private readonly IVaultClient _vaultClient;
        private readonly string _secretsPath;
        private readonly string _mountPoint;

        public VaultSecretsProvider(IVaultClient vaultClient, string secretsPath, string mountPoint)
        {
            _vaultClient = vaultClient;
            _secretsPath = secretsPath;
            _mountPoint = mountPoint;
        }

        public async Task<Dictionary<string, string>> GetSecretsAsync()
        {
            var secret = await _vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(
                path: _secretsPath,
                mountPoint: _mountPoint
            );

            var data = secret.Data.Data;
            var dict = new Dictionary<string, string>();

            foreach (var kv in data)
            {
                dict[kv.Key] = kv.Value?.ToString() ?? "";
            }

            return dict;
        }

        public async Task<string?> GetSecretAsync(string key)
        {
            var all = await GetSecretsAsync();
            all.TryGetValue(key, out var val);
            return val;
        }
    }
}
