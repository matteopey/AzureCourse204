using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Storage.Blobs;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;
using Azure.Core;

namespace Lab07
{
    public static class FileParser
    {
        [FunctionName("FileParser")]
        public static async Task<IActionResult> Run([HttpTrigger("GET")] HttpRequest req, ILogger logger)
        {
            var connStr = Environment.GetEnvironmentVariable("StorageConnectionString");

            if (connStr == "[TEST VALUE]")
            {
                var secretStore = Environment.GetEnvironmentVariable("SecretStore");
                var cred = new DefaultAzureCredential(new DefaultAzureCredentialOptions
                {
                    TenantId = "",
                });
                var secretClient = new SecretClient(new Uri(secretStore), cred);
                var secret = await secretClient.GetSecretAsync("storagecredentials");
                connStr = secret?.Value?.Value;
            }

            var blobClient =  new BlobClient(connStr, "drop", "records.json");
            var response = await blobClient.DownloadStreamingAsync();

            return new FileStreamResult(response.Value.Content, response.Value.Details.ContentType);
        }
    }
}
