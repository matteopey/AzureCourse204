using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

[StorageAccount("AzureWebJobsStorage")]
public static class GetSettingInfo
{
    [FunctionName("GetSettingInfo")]
    [return: Queue("queue")]
    public static string Run(
        [HttpTrigger("GET")] HttpRequest request,
        ILogger logger,
        [Blob("content/settings.json")] string json)
    {
        logger.LogInformation("Sending to queue");
        return json;
    }
}