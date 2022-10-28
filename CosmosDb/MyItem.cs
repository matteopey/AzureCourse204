namespace CosmosDb;

public class MyItem
{
    public Guid id { get; set; }
    public int categoryId { get; set; }
    public string? description { get; set; }
    public double price { get; set; }
    public string? categoryName { get; set; }
}