using CosmosDb;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

string url = "";
string key = "";

var client = new CosmosClient(url, key);
var accountInfo = await client.ReadAccountAsync();

Console.WriteLine(accountInfo.Consistency.DefaultConsistencyLevel);

var container = client.GetContainer("activities", "todos");

var newItem = new MyItem
{
	categoryId = 1,
	categoryName = "My category",
	description = "Description 123",
	id = Guid.NewGuid(),
	price = 12.4
};

var response = await container.CreateItemAsync(newItem);
Console.WriteLine(response.StatusCode);

var rand = new Random();
for (int i = 0; i < 500; i++)
{
	var item = new MyItem
	{
		categoryId = i % 2 == 0 ? 1 : 2,
		categoryName = "My category",
		description = "My Desc " + 1,
		id = Guid.NewGuid(),
		price = rand.NextDouble() * 10
	};

	Console.WriteLine($"Create item {i}");

	//await container.CreateItemAsync(item);
}

//var feedIterator = container.GetItemQueryIterator<MyItem>("SELECT * FROM activities a WHERE a.categoryid = 2");

var feedIterator = container.GetItemLinqQueryable<MyItem>()
	.Where(a => a.categoryId == 2)
	.OrderByDescending(a => a.price)
	.ToFeedIterator();

while (feedIterator.HasMoreResults)
{
	var feed = await feedIterator.ReadNextAsync();
	foreach (var item in feed)
	{
		Console.WriteLine($"{item.id} {item.categoryId} {item.price}");
	}
}

response = await container.CreateItemAsync(newItem, requestOptions: new ItemRequestOptions
{
	PreTriggers = new List<string> { "trgTimestampValidation" }
});
Console.WriteLine(response.StatusCode);