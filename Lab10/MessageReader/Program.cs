using Azure.Messaging.ServiceBus;

const string serviceBusConnectionString = "Endpoint=sb://matteopeysbnamespace.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=VkVNnPrhst+PvOE9g7suV0f/tsWyFkal6ML2aEBEqZQ=";
const string queueName = "myqueue";

ServiceBusClient client = new ServiceBusClient(serviceBusConnectionString);
ServiceBusProcessor processor = client.CreateProcessor(queueName, new ServiceBusProcessorOptions());

try
{
    processor.ProcessMessageAsync += MessageHandler;
    processor.ProcessErrorAsync += ErrorHandler;

    await processor.StartProcessingAsync();

    Console.ReadKey();

    Console.WriteLine("\nStopping the receiver...");
    await processor.StopProcessingAsync();
    Console.WriteLine("Stopped receiving messages");
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
finally
{
    await processor.DisposeAsync();
    await client.DisposeAsync();
}

async Task MessageHandler(ProcessMessageEventArgs args)
{
    var rand = new Random().NextDouble();
    if (rand > 0.9)
    {
        throw new Exception("New exception");
    }

    string body = args.Message.Body.ToString();
    Console.WriteLine($"Received: {body}");
    await args.CompleteMessageAsync(args.Message);
}

Task ErrorHandler(ProcessErrorEventArgs args)
{
    Console.WriteLine(args.Exception.ToString());
    return Task.CompletedTask;
}