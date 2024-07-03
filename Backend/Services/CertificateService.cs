// using Amazon.Lambda.Core;

using Amazon.SecretsManager.Model;
using Amazon.SecretsManager;
using Amazon;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;

namespace Backend.Services
{
    public class CertificateService
    {
        private static readonly string secretName = "Certificate_PFX";
        private readonly IAmazonSecretsManager secretsManagerClient;
        public CertificateService()
        {
            secretsManagerClient = new AmazonSecretsManagerClient(RegionEndpoint.EUWest1);
        }

        public async Task<X509Certificate2> GetCertAndKey()
        {
            GetSecretValueRequest request = new GetSecretValueRequest
            {
                SecretId = secretName,
                VersionStage = "AWSCURRENT",
            };

            GetSecretValueResponse resp = new GetSecretValueResponse();

            try
            {
                resp = await secretsManagerClient.GetSecretValueAsync(request);
            }
            catch (Exception ex)
            {
            }

            var secretObject = JsonConvert.DeserializeObject<Dictionary<string, string>>(resp.SecretString);

            if (secretObject.TryGetValue("pfx", out string pfxBase64) &&
                secretObject.TryGetValue("password", out string password))
            {
                // Decode base64 string to byte array
                byte[] pfxBytes = Convert.FromBase64String(pfxBase64);
                X509Certificate2 cert = new X509Certificate2(pfxBytes, password);
                return cert;
            }
            else
            {
                throw new Exception("PFX certificate or password not found in secret.");
            }
        }
    }
}