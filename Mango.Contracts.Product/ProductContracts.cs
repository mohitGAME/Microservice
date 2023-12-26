namespace Mango.Contracts.Product
{
    public record ProductCreated(int ProductId, string Name, double Price, string Description, string CategoryName, string ImageUrl);
    public record ProductUpdated(int ProductId, string Name, double Price, string Description, string CategoryName, string ImageUrl);
    public record ProductDeleted(int ProductId);
}


