using Azure;
using Azure.Messaging.EventGrid;
using System.Diagnostics;

public class Program
{
    private const string topicEndpoint = "";
    private const string topicKey = "";

    public static async Task Main(string[] args)
    {
        var endpoint = new Uri(topicEndpoint);
        var credential = new AzureKeyCredential(topicKey);
        var client = new EventGridPublisherClient(endpoint, credential);

        var firstEvent = new EventGridEvent(
            subject: $"New Employee: Alba Sutton",
            eventType: "Employees.Registration.New",
            dataVersion: "1.0",
            data: new
            {
                FullName = "Alba Sutton",
                Address = "4567 Pine Avenue, Edison, WA 97202"
            }
        );

        var secondEvent = new EventGridEvent(
            subject: $"New Employee: Alexandre Doyon",
            eventType: "Employees.Registration.New",
            dataVersion: "1.0",
            data: new
            {
                FullName = "Alexandre Doyon",
                Address = "456 College Street, Bow, WA 98107"
            }
        );

        await client.SendEventAsync(firstEvent);
        await Console.Out.WriteLineAsync("First event published");

        await client.SendEventAsync(secondEvent);
        await Console.Out.WriteLineAsync("Second event published");

        var timing = new Stopwatch();
        timing.Start();
        for (int i = 0; i < 100; i++)
        {
            await client.SendEventAsync(new EventGridEvent(
                subject: $"New Employee {i}",
                eventType: "Employees.Registration.New",
                dataVersion: "1.0",
                data: new
                {
                    FullName = "Alba Sutton",
                    Address = "4567 Pine Avenue, Edison, WA 97202"
                }
            ));
        }

        timing.Stop();
        await Console.Out.WriteLineAsync($"Took {timing.ElapsedMilliseconds}ms to send 100 events singularly");

        timing.Restart();
        var list = Enumerable
            .Range(0, 100)
            .Select(i => new EventGridEvent(
                subject: $"New Employee {i}",
                eventType: "Employees.Registration.New",
                dataVersion: "1.0",
                data: new
                {
                    FullName = "Alba Sutton",
                    Address = "4567 Pine Avenue, Edison, WA 97202"
                }
            ));

        timing.Stop();
        await Console.Out.WriteLineAsync($"Took {((double)timing.ElapsedTicks / Stopwatch.Frequency) * 1000000}us to send 100 events at the same time");

        await client.SendEventsAsync(list);
    }
}