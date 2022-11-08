using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

const string serviceBusConnectionString = "Endpoint=sb://matteopeysbnamespace.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=VkVNnPrhst+PvOE9g7suV0f/tsWyFkal6ML2aEBEqZQ=";
const string queueName = "myqueue";
const int numOfMessages = 300;

ServiceBusClient client = new ServiceBusClient(serviceBusConnectionString);
ServiceBusSender sender = client.CreateSender(queueName);

using var messageBatch = await sender.CreateMessageBatchAsync();

for (int i = 1; i <= numOfMessages; i++)
{
    if (!messageBatch.TryAddMessage(new ServiceBusMessage($"Message {i}")))
    {
        throw new Exception($"The message {i} is too large to fit in the batch.");
    }
}

try
{
    await sender.SendMessagesAsync(messageBatch);
    Console.WriteLine($"A batch of {numOfMessages} messages has been published to the queue.");
}
finally
{
    await sender.DisposeAsync();
    await client.DisposeAsync();
}