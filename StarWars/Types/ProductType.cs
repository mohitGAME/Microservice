using GraphQL.Types;

namespace StarWars.Types
{
    public class ProductType : ObjectGraphType<Product>
    {
        public ProductType()
        {
            Name = "Product";
            Description = "An article or substance that is manufactured or refined for sale.";

            Field(h => h.ProductId).Description("The id of the product.");
            Field(h => h.Name, nullable: true).Description("The name of the product.");
            Field(h => h.Description, nullable: true).Description("The description of the product.");
        }
    }
}



