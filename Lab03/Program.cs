using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Threading.Tasks;

const string blobServiceEndpoint = "";
const string storageAccountName = "";
const string storageAccountKey = "";
const string storageAccountConnStr = "";

StorageSharedKeyCredential accountCredentials = new StorageSharedKeyCredential(storageAccountName, storageAccountKey);
BlobServiceClient serviceClient = new BlobServiceClient(new Uri(blobServiceEndpoint), accountCredentials);
AccountInfo info = await serviceClient.GetAccountInfoAsync();
await Console.Out.WriteLineAsync($"Connected to Azure Storage Account");
await Console.Out.WriteLineAsync($"Account name:\t{storageAccountName}");
await Console.Out.WriteLineAsync($"Account kind:\t{info?.AccountKind}");
await Console.Out.WriteLineAsync($"Account sku:\t{info?.SkuName}");

await EnumerateContainersAsync(serviceClient);

// Create a container
string newContainerName = "vector-graphics";
var blobContainerClient = await GetContainerAsync(serviceClient, newContainerName);

// Upload a blob from a local file
var blobClient = new BlobClient(storageAccountConnStr, newContainerName, "newblob2.svg");
// await blobClient.UploadAsync("C:\\Users\\matteo.peyronel\\Documents\\Coding\\AZ-204-DevelopingSolutionsforMicrosoftAzure\\Allfiles\\Labs\\03\\Starter\\Images\\graph.svg");

// var metadata = new Dictionary<string, string>
// {
//     { "meta1", "val1" }
// };

// await blobClient.SetMetadataAsync(metadata);

var resp = await blobClient.GetPropertiesAsync();
foreach (var meta in resp.Value.Metadata)
{
    await Console.Out.WriteLineAsync($"Meta:\t\t{meta.Key}, {meta.Value}");
}

await blobClient.SetTagsAsync(new Dictionary<string, string> { { "sdf", "fsd" } });
var tags = await blobClient.GetTagsAsync();
foreach (var meta in tags.Value.Tags)
{
    await Console.Out.WriteLineAsync($"Tag:\t\t{meta.Key}, {meta.Value}");
}

var blob = await GetBlobAsync(blobContainerClient, "newblob2.svg");
await Console.Out.WriteLineAsync($"Blob Url:\t{blob.Uri}");

async Task EnumerateContainersAsync(BlobServiceClient client)
{
    // Enumerate containers
    await foreach (BlobContainerItem container in client.GetBlobContainersAsync())
    {
        await Console.Out.WriteLineAsync($"Container:\t{container.Name}");

        await EnumerateBlobsAsync(client, container.Name);
    }
}

async Task EnumerateBlobsAsync(BlobServiceClient client, string containerName)
{
    BlobContainerClient container = client.GetBlobContainerClient(containerName);

    await Console.Out.WriteLineAsync($"Searching:\t{container.Name}");

    await foreach (BlobItem blob in container.GetBlobsAsync())
    {
        await Console.Out.WriteLineAsync($"Existing Blob:\t{blob.Name}");
    }
}

async Task<BlobContainerClient> GetContainerAsync(BlobServiceClient client, string containerName)
{
    BlobContainerClient container = client.GetBlobContainerClient(containerName);
    await container.CreateIfNotExistsAsync(PublicAccessType.Blob);
    await Console.Out.WriteLineAsync($"New Container:\t{container.Name}");
    return container;
}

async Task<BlobClient> GetBlobAsync(BlobContainerClient client, string blobName)
{
    BlobClient blob = client.GetBlobClient(blobName);
    await Console.Out.WriteLineAsync($"Blob Found:\t{blob.Name}");
    return blob;
}