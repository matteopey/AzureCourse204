using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

public static class Echo
{
    [FunctionName("Echo")]
    public static async Task<IActionResult> Run([HttpTrigger("POST")] HttpRequest request, ILogger logger)
    {
        logger.LogInformation("Received a request");

        return new OkObjectResult(request.Body);
    }
}